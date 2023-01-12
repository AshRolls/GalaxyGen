using GalaxyGenEngine.Engine.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Controllers
{
    public interface IAgentController
    {
        void ReceiveCommand(MessageAgentCommand msg);
        void Tick(MessageTick tick);
    }
}
