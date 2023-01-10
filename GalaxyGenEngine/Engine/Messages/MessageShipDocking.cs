using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessageShipDocking : IMessageShipCommandData
    {
        public MessageShipDocking(ShipCommandEnum type, Int64 dockingTargetId)
        {
            CommandType = type;
            DockingTargetId = dockingTargetId;
        }

        public ShipCommandEnum CommandType { get; set; }

        public Int64 DockingTargetId { get; set; }
    }
}
