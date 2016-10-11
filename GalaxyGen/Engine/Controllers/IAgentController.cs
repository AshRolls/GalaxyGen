using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public interface IAgentController
    {
        object Tick(MessageTick tick);
        void ReceiveShipResponse(MessageShipResponse msg);
    }
}
