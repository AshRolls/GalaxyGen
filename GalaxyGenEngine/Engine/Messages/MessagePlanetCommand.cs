using GCEngine.Model;
using GCEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Messages
{
    public enum PlanetCommandEnum
    {
        RequestResourceShip
    }

    public class MessagePlanetCommand : Message
    {
        public MessagePlanetCommand(IMessagePlanetCommandData cmd, Int64 tickSent, Int64 planetId)
        {
            Command = cmd;
            TickSent = tickSent;
            PlanetId = planetId;
        }

        public IMessagePlanetCommandData Command { get; private set; }        
        public Int64 PlanetId { get; private set; }
    }
}
