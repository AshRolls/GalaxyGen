using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGenCore.Resources;

namespace GalaxyGenEngine.Engine.Controllers
{
    public interface IAgentActions
    {
        void RequestUndock();
        void RequestDock();
        void RequestLoadShip(ResourceQuantity resQ);
        void RequestUnloadShip(ResourceQuantity resQ);
        void RequestCreateSellOrder(ResourceQuantity resQ);
        void RequestCreateBuyOrder(ResourceQuantity resQ);
    }
}
