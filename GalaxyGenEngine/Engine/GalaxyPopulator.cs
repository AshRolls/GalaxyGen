using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyGenEngine.Model;
using GalaxyGenCore.StarChart;
using GalaxyGenCore.BluePrints;
using GalaxyGenCore.Resources;

namespace GalaxyGenEngine.Engine
{
    public class GalaxyPopulator : IGalaxyPopulator
    {
        const int NUMBER_OF_AGENTS = 100;
        private Galaxy _gal;
        public Galaxy GetFullGalaxy()
        {
            _gal = this.getNewGalaxy();
            _gal.MaxId = 100;
            Currency currency = new Currency();
            currency.Name = "GalStandard";
            _gal.Currencies.Add(currency.CurrencyId, currency);

            ShipType shipT = new ShipType();
            shipT.Name = "Basic Ship";
            shipT.MaxCruisingSpeedKmH = 300000;
            _gal.ShipTypes.Add(shipT);
                        
            foreach (ScSolarSystem chartSS in StarChart.SolarSystems.Values)
            {
                SolarSystem ss = getNewSolarSystemFromStarChart(chartSS);
                ss.StarChartId = StarChart.GetIdForObject(chartSS);
                

                foreach (ScPlanet chartP in chartSS.Planets)
                {
                    Planet p = this.getNewPlanet(chartP, currency.CurrencyId);
                    p.StarChartId = StarChart.GetIdForObject(chartP);
                    ss.Planets.Add(p.StarChartId, p);
                    p.SolarSystem = ss;
                }
                
                for (int i = 0; i < NUMBER_OF_AGENTS; i++)
                {
                    AddAgent(shipT, chartSS, i, ss, currency.CurrencyId);
                }

                _gal.SolarSystems.Add(ss);
            }

            return _gal;
        }

        private void AddAgent(ShipType shipT, ScSolarSystem chartSS, int name, SolarSystem ss, ulong seedCurrencyId)
        {
            Agent ag = this.GetAgent("Agent " + name, seedCurrencyId);
            ss.Agents.Add(ag);
            ag.SolarSystem = ss;

            int j = 0;
            List<ResourceQuantity> resQs = new() { new ResourceQuantity(ResourceTypeEnum.Exotic_Spice, 10), 
                                               new ResourceQuantity(ResourceTypeEnum.Metal_Platinum, 10),
                                               new ResourceQuantity(ResourceTypeEnum.Metal_Uranium, 10),
                                               new ResourceQuantity(ResourceTypeEnum.Gas_Xenon, 10),
                                               new ResourceQuantity(ResourceTypeEnum.Metal_Aluminium, 10)
            };
            foreach (Planet p in ss.Planets.Values)
            {
                if (j % 2 == 0)
                {
                    addMetalProducerToPlanet(ag, p);
                    addNewStoreToPlanet(p, ag, resQs);
                    //addNewStoreToPlanet(p, ag, new List<ResourceQuantity>());
                }
                else
                {
                    addSpiceProducerToPlanet(ag, p);
                    addNewStoreToPlanet(p, ag, resQs);
                    //addNewStoreToPlanet(p, ag, new List<ResourceQuantity>());
                }
                j++;
            }                            

            Ship s = this.getNewShip("Ship" + name, shipT);
            s.Owner = ag;
            s.ShipState = ShipStateEnum.Docked;
            s.Agents.Add(ag);
            ag.Location = s;
            s.Pilot = ag;
            ag.AgentState = AgentStateEnum.PilotingShip;
            s.DockedPlanet = ss.Planets.Values.First();
            ss.Planets.Values.First().DockedShips.Add(s.ShipId, s);
            ag.ShipsOwned.Add(s);
            addNewCargoStoreToShip(s, ag);
            s.SolarSystem = ss;
            ss.Ships.Add(s.ShipId, s);
        }

        private void addMetalProducerToPlanet(Agent ag, Planet p)
        {
            Producer prod = this.getNewProducer("Factory Metal", BluePrintEnum.SpiceToPlatinum);
            prod.Owner = ag;
            prod.Planet = p;
            ag.Producers.Add(prod);
            p.Producers.Add(prod);
        }

        private void addSpiceProducerToPlanet(Agent ag, Planet p)
        {           
            Producer prod = this.getNewProducer("Factory Spice", BluePrintEnum.PlatinumToSpice);
            prod.Owner = ag;
            prod.Planet = p;
            ag.Producers.Add(prod);
            p.Producers.Add(prod);
        }

        private SolarSystem getNewSolarSystemFromStarChart(ScSolarSystem chartSS)
        {
            SolarSystem ss = new SolarSystem();
            ss.Name = chartSS.Name;           
            return ss;
        }

        private Galaxy getNewGalaxy()
        {
            Galaxy gal = new Galaxy();
            gal.Name = "Milky Way";
            return gal;
        }

        private Planet getNewPlanet(ScPlanet chartP, ulong currencyId)
        {
            Planet plan = new Planet();
            plan.Name = chartP.Name;
            plan.Population = 10000;

            Society soc = new Society();
            soc.Name = chartP.Name + " Soc";
            plan.Society = soc;

            Market m = new Market();
            m.CurrencyId = currencyId;
            plan.Market = m;
            
            return plan;
        }

        private void addNewStoreToPlanet(Planet p, Agent o, List<ResourceQuantity> resourcesToSeed)
        {
            Store s = new Store();
            s.OwnerId = o.AgentId;
            s.Location = p;
            p.Stores.Add(o.AgentId,s);
            o.Stores.Add(s);

            foreach(ResourceQuantity resQ in resourcesToSeed)
            {
                s.StoredResources.Add(resQ.Type, resQ.Quantity);
            }                        
        }

        private Agent GetAgent(String seedName, ulong seedCurrencyId)
        {
            Agent ag = new Agent();
            ag.Memory = "";
            ag.AccountId  = getAccount(ag, seedCurrencyId);                       
            ag.Name = seedName;
            return ag;
        }

        private ulong getAccount(Agent owner, ulong seedCurrencyId)
        {
            Account ac = new Account();
            ac.Balances.Add(seedCurrencyId, 10000L);
            ac.Owner = owner;
            _gal.Accounts.Add(owner.AgentId, ac);
            return ac.AccountId;
        }

        private Producer getNewProducer(String seedName, BluePrintEnum bpType)
        {
            Producer prod = new Producer();
            prod.Name = seedName;
            prod.BluePrintType = bpType;
            prod.Producing = false;
            prod.AutoResumeProduction = true;
            prod.ProduceNThenStop = 0;
            return prod;
        }

        private void addNewCargoStoreToShip(Ship ship, Agent o)
        {
            Store s = new Store();
            s.OwnerId = o.AgentId;
            s.Location = ship;
            ship.Stores.Add(o.AgentId,s);           
            o.Stores.Add(s);

            // seed with basic starter resource
            s.StoredResources.Add(ResourceTypeEnum.Exotic_Spice, 2);
            s.StoredResources.Add(ResourceTypeEnum.Metal_Platinum, 2);
        }

        private Ship getNewShip(String seedName, ShipType shipT)
        {
            Ship s = new Ship();
            s.Type = shipT;
            s.Name = seedName;                        
            return s;
        }

        //public ResourceQuantity GetResourceQuantity(ResourceTypeEnum type, Int64 qty)
        //{
        //    ResourceQuantity resQ = new ResourceQuantity();
        //    resQ.Type = type;
        //    resQ.Quantity = qty;
        //    return resQ;
        //}     

    }
}
