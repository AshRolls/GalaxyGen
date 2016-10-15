using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using GalaxyGenCore;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Messages
{
    public class MessageAgentDestinationReached : Message
    {
        public MessageAgentDestinationReached(Int64 tickSent)
        {
            TickSent = tickSent;
        }        
    }
}
