using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Messages
{
    public class MessageShipSetDestination : IMessageShipCommandData
    {
        public MessageShipSetDestination(ShipCommandEnum type, Int64 desinationScId)
        {
            CommandType = type;
            DestinationScId = desinationScId;
        }

        public ShipCommandEnum CommandType { get; set; }

        public Int64 DestinationScId { get; set; }
    }
}
