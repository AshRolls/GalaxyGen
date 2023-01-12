using System;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum AgentCommandEnum
    {
        ShipCommandFailed,
        PlanetCommandFailed
    }

    public class MessageAgentCommand : Message
    {
        public MessageAgentCommand(IMessageAgentCommandData cmd, UInt64 tickSent)
        {
            Command = cmd;
            TickSent = tickSent;
        }

        public IMessageAgentCommandData Command { get; private set; }        
    }
}
