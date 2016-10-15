using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Messages
{
    public class MessageShipBasic : IMessageShipCommandData
    {
        public MessageShipBasic(ShipCommandEnum type)
        {
            CommandType = type;
        }

        public ShipCommandEnum CommandType { get; set; }
    }
}
