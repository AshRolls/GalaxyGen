using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public enum ShipCommandEnum
    {
        Undock,
        Dock
    }

    public class MessageShipCommand : Message
    {
        public MessageShipCommand(ShipCommandEnum cmd, Int64 tickSent, Int64 shipId)
        {
            Command = cmd;
            TickSent = tickSent;
            ShipId = shipId;
        }

        public ShipCommandEnum Command { get; private set; }        
        public Int64 ShipId { get; private set; }
    }
}
