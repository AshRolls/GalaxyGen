using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Messages
{
    public interface IMessageShipCommandData
    {
        ShipCommandEnum CommandType { get; set; }
    }
}
