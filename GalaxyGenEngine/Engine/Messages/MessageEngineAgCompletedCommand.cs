using System;

namespace GalaxyGenEngine.Engine.Messages
{
    internal class MessageEngineAgCompletedCommand
    {
        public MessageEngineAgCompletedCommand(UInt64 agentId)
        {
            AgentId = agentId;
        }

        public UInt64 AgentId { get; private set; }
    }
}