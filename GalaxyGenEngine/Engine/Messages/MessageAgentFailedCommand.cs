using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessageAgentFailedCommand : IMessageAgentCommandData
    {
        public MessageAgentFailedCommand(AgentCommandEnum type)
        {
            CommandType = type;            
        }

        public AgentCommandEnum CommandType { get; set; }
    }
}
