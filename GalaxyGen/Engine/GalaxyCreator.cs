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
        public IPlanet GetPlanet()
        {
            StandardKernel kernel = Bindings.Kernel;

            IPlanet plan = kernel.Get<IPlanet>();
            plan.Population = 10000;
            plan.Name = "Earth";
            ISociety soc = kernel.Get<ISociety>();
            plan.Society = soc;
            IMarket mar = kernel.Get<IMarket>();            
            
            IMarketBuyOrder mbo = kernel.Get<IMarketBuyOrder>();
            IAgent agent = kernel.Get<IAgent>();
            agent.Name = "Agent 1";
            mbo.Owner = agent;
            mbo.Quantity = 5;
            mbo.Type = ResourceTypeEnum.Spice;
            mar.BuyOrders.Add(mbo);

            plan.Market = mar;

            return plan;
        }
    }
}
