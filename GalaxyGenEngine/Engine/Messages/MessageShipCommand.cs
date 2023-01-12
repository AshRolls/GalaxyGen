using System;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum ShipCommandEnum
    {
        SetXY,
        Undock,
        Dock,
        SetDestination,
        SetAutopilot
    }

    public class MessageShipCommand : Message
    {
        public MessageShipCommand(IMessageShipCommandData cmd, UInt64 tickSent, UInt64 shipId, UInt64 agentId)
        {
            Command = cmd;
            TickSent = tickSent;
            ShipId = shipId;
            AgentId = agentId;
        }

        public IMessageShipCommandData Command { get; private set; }        
        public UInt64 ShipId { get; private set; }
        public UInt64 AgentId { get; private set; }
    }
}
