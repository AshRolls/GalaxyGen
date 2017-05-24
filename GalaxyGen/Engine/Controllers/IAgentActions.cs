using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Controllers
{
    public interface IAgentActions
    {
        void RequestUndock();
        void RequestDock();

    }
}
