using System;

namespace GalaxyGen.Engine.Messages
{
    internal class MessageEngineAgCompletedCommand
    {
        public MessageEngineAgCompletedCommand(Int64 agentId, Int64 tick)
        {
            AgentId = agentId;
            Tick = tick;
        }

        public Int64 AgentId { get; private set; }
        public Int64 Tick { get; private set; }
    }
}