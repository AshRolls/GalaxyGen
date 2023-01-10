using System;

namespace GalaxyGenEngine.Engine.Messages
{
    internal class MessageEngineAgCompletedCommand
    {
        public MessageEngineAgCompletedCommand(Int64 agentId)
        {
            AgentId = agentId;
        }

        public Int64 AgentId { get; private set; }
    }
}