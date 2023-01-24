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

    public record MessageShipCommand(IMessageShipCommandData Command, UInt64 TickSent, UInt64 ShipId, UInt64 AgentId) : Message(TickSent);

    public record MessageShipBasic(ShipCommandEnum CommandType, ulong TargetId) : IMessageShipCommandData;
    public record MessageShipDocking(ShipCommandEnum CommandType, ulong DockingTargetId) : IMessageShipCommandData;
    public record MessageShipSetAutopilot(ShipCommandEnum CommandType, bool Active) : IMessageShipCommandData;
    public record MessageShipSetDestination(ShipCommandEnum CommandType, UInt64 DestinationScId) : IMessageShipCommandData;
    public record MessageShipSetXY(ShipCommandEnum CommandType, Double X, Double Y) : IMessageShipCommandData;
    public record MessageShipResponse(bool Response, MessageShipCommand SentCommand, UInt64 TickSent) : Message(TickSent);        
}
