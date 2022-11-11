using GCEngine.Engine.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Controllers
{
    public interface IAgentController
    {
        void Tick(MessageTick tick);
    }
}
