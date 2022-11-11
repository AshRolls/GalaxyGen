using GCEngine.Model;
using GCEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Messages
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
        public MessageShipCommand(IMessageShipCommandData cmd, Int64 tickSent, Int64 shipId)
        {
            Command = cmd;
            TickSent = tickSent;
            ShipId = shipId;
        }

        public IMessageShipCommandData Command { get; private set; }        
        public Int64 ShipId { get; private set; }
    }
}
