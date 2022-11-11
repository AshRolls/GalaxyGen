using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Messages
{
    public class MessageShipSetAutopilot : IMessageShipCommandData
    {
        public MessageShipSetAutopilot(ShipCommandEnum type, bool active)
        {
            CommandType = type;
            Active = active;
        }

        public ShipCommandEnum CommandType { get; set; }

        public bool Active { get; set; }
    }
}
