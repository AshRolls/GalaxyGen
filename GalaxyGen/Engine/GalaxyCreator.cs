using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGen.Model;
using Ninject;
using GalaxyGen.Framework;

namespace GalaxyGen.Engine
{
    public class GalaxyCreator : IGalaxyCreator
    {
        public Galaxy GetFullGalaxy()
        {
            Galaxy gal = this.GetGalaxy();

            SolarSystem ss = this.GetSolarSystem("Sol");

            Agent ag = this.GetAgent("The Mule");
            Producer prod = this.GetProducer("Factory Metal", BluePrintEnum.SpiceToPlatinum);
            prod.Owner = ag;
            ag.Producers.Add(prod);
            ss.Agents.Add(ag);

            Agent ag2 = this.GetAgent("The Shrike");
            Producer prod2 = this.GetProducer("Factory Harkonen", BluePrintEnum.PlatinumToSpice);
            prod2.Owner = ag2;
            ag2.Producers.Add(prod2);
            ss.Agents.Add(ag2);

            Planet p = this.GetPlanet("Earth");
            p.Producers.Add(prod);
            addNewStoreToPlanet(p, ag);            
            p.Producers.Add(prod2);
            addNewStoreToPlanet(p, ag2);
            ss.Planets.Add(p);

            Planet p2 = this.GetPlanet("Mars");
            
            ss.Planets.Add(p2);
            gal.SolarSystems.Add(ss);

            //for (int i = 0; i < 500; i++)
            //{
            //    Agent ag3 = _galaxyCreator.GetAgent(i.ToString());
            //    ss.Agents.Add(ag3);
            //}

            //for (int i = 0; i < 500; i++)
            //{
            //    SolarSystem ss2 = _galaxyCreator.GetSolarSystem(i.ToString());
            //    ss2.Planets.Add(_galaxyCreator.GetPlanet("Earth"));
            //    ss2.Planets.Add(_galaxyCreator.GetPlanet("Mars"));
            //    gal.SolarSystems.Add(ss2);
            //}

            return gal;
        }

        public Galaxy GetGalaxy()
        {
            Galaxy gal = new Galaxy();
            gal.Name = "Milky Way";
            gal.SolarSystems = new HashSet<SolarSystem>();
            return gal;
        }

        public SolarSystem GetSolarSystem(string seedName)
        {
            SolarSystem sys = new SolarSystem();
            sys.Name = seedName;
            sys.Planets = new List<Planet>();
            sys.Agents = new List<Agent>();
            return sys;
        }

        public Planet GetPlanet(string seedName)
        {
            Planet plan = new Planet();
            plan.Population = 10000;
            plan.Name = seedName;
            Society soc = new Society();
            soc.Name = seedName + " Soc";
            plan.Society = soc;
            plan.Producers = new HashSet<Producer>();
            plan.Stores = new HashSet<Store>();

            //IMarket mar = kernel.Get<IMarket>();            
            
            //IMarketBuyOrder mbo = kernel.Get<IMarketBuyOrder>();
            //IAgent agent = kernel.Get<IAgent>();
            //agent.Name = "Agent 1";
            //mbo.Owner = agent;
            //mbo.Quantity = 5;
            //mbo.Type = ResourceTypeEnum.Spice;
            //mar.BuyOrders.Add(mbo);

            //plan.Market = mar;

            return plan;
        }

        private void addNewStoreToPlanet(Planet p, Agent o)
        {
            Store s = new Store();
            s.StoredResources = new HashSet<ResourceQuantity>();
            s.Owner = o;
            s.Location = p;            
            p.Stores.Add(s);
            o.Stores.Add(s);

            ResourceQuantity resQ = new ResourceQuantity(); // seed with basic starter resource
            resQ.Type = ResourceTypeEnum.Spice;
            resQ.Quantity = 40;
            s.StoredResources.Add(resQ);
        }

        public Agent GetAgent(String seedName)
        {
            Agent ag = new Agent();
            ag.Name = seedName;
            ag.Producers = new HashSet<Producer>();
            ag.Stores = new HashSet<Store>();
            return ag;
        }

        public Producer GetProducer(String seedName, BluePrintEnum bpType)
        {
            Producer prod = new Producer();
            prod.Name = seedName;
            prod.BluePrintType = bpType;
            prod.TicksCompleted = 0;
            return prod;
        }       

    }
}
