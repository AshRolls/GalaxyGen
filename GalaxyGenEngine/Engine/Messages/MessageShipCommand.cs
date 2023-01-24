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

    public record MessageShipBasic(ShipCommandEnum CommandType, ulong TargetId) : IMessageShipCommandData;
    public record MessageShipDocking(ShipCommandEnum CommandType, ulong DockingTargetId) : IMessageShipCommandData;
    public record MessageShipSetAutopilot(ShipCommandEnum CommandType, bool Active) : IMessageShipCommandData;
    public record MessageShipSetDestination(ShipCommandEnum CommandType, UInt64 DestinationScId) : IMessageShipCommandData;
    public record MessageShipSetXY(ShipCommandEnum CommandType, Double X, Double Y) : IMessageShipCommandData;

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
