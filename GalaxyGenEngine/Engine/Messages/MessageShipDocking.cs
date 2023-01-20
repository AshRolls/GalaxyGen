using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessageShipDocking : IMessageShipCommandData
    {
        public MessageShipDocking(ShipCommandEnum type, UInt64 dockingTargetId)
        {
            CommandType = type;
            DockingTargetId = dockingTargetId;
        }

        public ShipCommandEnum CommandType { get; set; }

        public UInt64 DockingTargetId { get; set; }
    }
}
