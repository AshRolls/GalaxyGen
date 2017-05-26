using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGen.Model;
using Ninject;
using GalaxyGen.Framework;
using GalaxyGenCore;
using GalaxyGenCore.StarChart;
using GalaxyGenCore.BluePrints;
using GalaxyGenCore.Resources;

namespace GalaxyGen.Engine
{
    public class GalaxyPopulator : IGalaxyPopulator
    {
        public Galaxy GetFullGalaxy()
        {
            Galaxy gal = this.GetGalaxy();
            gal.MaxId = 100;

            ShipType shipT = new ShipType();
            shipT.Name = "Basic Ship";
            shipT.MaxCruisingSpeedKmH = 300000;
            gal.ShipTypes.Add(shipT);
                        
            foreach (ScSolarSystem chartSS in StarChart.SolarSystems.Values)
            {
                SolarSystem ss = getSolarSystemFromStarChartSS(chartSS);
                ss.StarChartId = StarChart.GetIdForObject(chartSS);

                Agent ag = this.GetAgent("Agent " + chartSS.Name);                
                ss.Agents.Add(ag);
                ag.SolarSystem = ss;

                int j = 0;
                foreach (ScPlanet chartP in chartSS.Planets)
                {
                    Planet p = this.GetPlanet(chartP);
                    p.StarChartId = StarChart.GetIdForObject(chartP);

                    if (j % 2 == 0)
                    {
                        //addMetalProducerToPlanet(ag, p);
                        addNewStoreToPlanet(p, ag, new List<ResourceQuantity>() { new ResourceQuantity(ResourceTypeEnum.Spice, 10) });
                    }
                    else
                    {
                        //addSpiceProducerToPlanet(ag, p);
                        addNewStoreToPlanet(p, ag, new List<ResourceQuantity>() { new ResourceQuantity(ResourceTypeEnum.Platinum, 10) });
                    }

                    j++;
                    ss.Planets.Add(p);
                }

                Ship s = this.GetShip("Ship" + chartSS.Name, shipT);
                s.Owner = ag;
                s.ShipState = ShipStateEnum.Docked;
                s.Agents.Add(ag);
                ag.Location = s;
                s.Pilot = ag;
                ag.AgentState = AgentStateEnum.PilotingShip;
                s.DockedPlanet = ss.Planets.First();
                ss.Planets.First().DockedShips.Add(s);
                ag.ShipsOwned.Add(s);
                addNewCargoStoreToShip(s, ag);
                s.SolarSystem = ss;
                ss.Ships.Add(s);

                for (int i = 0; i < 1000; i++)
                {

                    ag = this.GetAgent("Agent " + i);
                    ss.Agents.Add(ag);
                    ag.SolarSystem = ss;

                    s = this.GetShip("Ship" + i, shipT);
                    s.Owner = ag;
                    s.ShipState = ShipStateEnum.Docked;
                    s.Agents.Add(ag);
                    ag.Location = s;
                    s.Pilot = ag;
                    ag.AgentState = AgentStateEnum.PilotingShip;
                    s.DockedPlanet = ss.Planets.First();
                    ss.Planets.First().DockedShips.Add(s);
                    ag.ShipsOwned.Add(s);
                    addNewCargoStoreToShip(s, ag);
                    s.SolarSystem = ss;
                    ss.Ships.Add(s);
                }

                gal.SolarSystems.Add(ss);
            }                      

            return gal;
        }

        private void addMetalProducerToPlanet(Agent ag, Planet p)
        {
            Producer prod = this.GetProducer("Factory Metal", BluePrintEnum.SpiceToPlatinum);
            prod.Owner = ag;
            ag.Producers.Add(prod);
            p.Producers.Add(prod);

        }

        private void addSpiceProducerToPlanet(Agent ag, Planet p)
        {           
            Producer prod2 = this.GetProducer("Factory Spice", BluePrintEnum.PlatinumToSpice);
            prod2.Owner = ag;
            ag.Producers.Add(prod2);
            p.Producers.Add(prod2);
        }


        private SolarSystem getSolarSystemFromStarChartSS(ScSolarSystem chartSS)
        {
            SolarSystem ss = new SolarSystem();
            ss.Name = chartSS.Name;           
            return ss;
        }

        private Galaxy GetGalaxy()
        {
            Galaxy gal = new Galaxy();
            gal.Name = "Milky Way";
            return gal;
        }


        private Planet GetPlanet(ScPlanet chartP)
        {
            Planet plan = new Planet();
            plan.Name = chartP.Name;
            plan.Population = 10000;

            Society soc = new Society();
            soc.Name = chartP.Name + " Soc";
            plan.Society = soc;

            return plan;
        }

        private void addNewStoreToPlanet(Planet p, Agent o, List<ResourceQuantity> resourcesToSeed)
        {
            Store s = new Store();
            s.Owner = o;
            s.Location = p;
            p.Stores.Add(o.AgentId,s);
            o.Stores.Add(s);

            foreach(ResourceQuantity resQ in resourcesToSeed)
            {
                s.StoredResources.Add(resQ.Type, resQ.Quantity);
            }                        
        }

        private Agent GetAgent(String seedName)
        {
            Agent ag = new Agent();
            ag.Memory = "";
            ag.Account = getAccount();
            ag.Account.Owner = ag;            
            ag.Name = seedName;
            return ag;
        }

        private Account getAccount()
        {
            Account ac = new Account();
            ac.Balance = 1000000;
            return ac;
        }

        private Producer GetProducer(String seedName, BluePrintEnum bpType)
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
            s.Owner = o;
            s.Location = ship;
            ship.Stores.Add(o.AgentId,s);           
            o.Stores.Add(s);

            // seed with basic starter resource
            s.StoredResources.Add(ResourceTypeEnum.Spice, 10);
        }

        private Ship GetShip(String seedName, ShipType shipT)
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
