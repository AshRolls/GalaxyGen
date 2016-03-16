using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGen.Model;
using Ninject;

namespace GalaxyGen.Engine
{
    public class GalaxyCreator : IGalaxyCreator
    {
        public Galaxy GetGalaxy()
        {
            Galaxy gal = new Galaxy();
            gal.Name = "Milky Way";
            gal.SolarSystems = new List<SolarSystem>();
            return gal;
        }

        public SolarSystem GetSolarSystem(string seedName)
        {
            SolarSystem sys = new SolarSystem();
            sys.Name = seedName;
            sys.Planets = new List<Planet>();
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
    }
}
