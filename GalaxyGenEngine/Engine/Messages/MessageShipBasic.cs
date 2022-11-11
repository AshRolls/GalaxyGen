using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Messages
{
    public class MessageShipBasic : IMessageShipCommandData
    {
        public MessageShipBasic(ShipCommandEnum type, Int64 targetId)
        {
            CommandType = type;
            TargetId = targetId;
        }

        public ShipCommandEnum CommandType { get; set; }

        public Int64 TargetId { get; set; }
    }
}
