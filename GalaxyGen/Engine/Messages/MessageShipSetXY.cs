using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Messages
{
    public class MessageShipSetXY : IMessageShipCommandData
    {
        public MessageShipSetXY(ShipCommandEnum type, Double x, Double y)
        {
            CommandType = type;
            X = x;
            Y = y;
        }

        public ShipCommandEnum CommandType { get; set; }

        public Double X { get; set; }
        public Double Y { get; set; }
    }
}
