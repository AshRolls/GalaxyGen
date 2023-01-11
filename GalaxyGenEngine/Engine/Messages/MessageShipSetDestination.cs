using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessageShipSetDestination : IMessageShipCommandData
    {
        public MessageShipSetDestination(ShipCommandEnum type, UInt64 desinationScId)
        {
            CommandType = type;
            DestinationScId = desinationScId;
        }

        public ShipCommandEnum CommandType { get; set; }

        public UInt64 DestinationScId { get; set; }
    }
}
