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
        public Planet GetPlanet()
        {
            Planet plan = new Planet();
            plan.Population = 10000;
            plan.Name = "Earth";
            Society soc = new Society();
            soc.Name = "Earth Soc";
            plan.Soc = soc;
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
