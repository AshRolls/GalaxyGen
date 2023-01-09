using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Messages
{
    public interface IMessagePlanetCommandData
    {
        PlanetCommandEnum CommandType { get; set; }
    }
}
