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

        public Agent GetAgent(String seedName)
        {
            Agent ag = new Agent();
            ag.Name = seedName;
            ag.Producers = new HashSet<Producer>();
            return ag;
        }

        public Producer GetProducer(String seedName, List<ResourceTypeEnum> produced, List<ResourceTypeEnum> consumed)
        {
            Producer prod = new Producer();
            prod.Name = seedName;
            List<int> producedInt = produced.Select(x => (int)x).ToList();
            prod.ResourcesProducedJsonSerialized = GalaxyJsonSerializer.Serialize(producedInt);
            List<int> consumedInt = consumed.Select(x => (int)x).ToList();
            prod.ResourcesConsumedJsonSerialized = GalaxyJsonSerializer.Serialize(consumedInt);
            return prod;
        }

    }
}
