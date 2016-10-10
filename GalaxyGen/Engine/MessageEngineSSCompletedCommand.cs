using System;

namespace GalaxyGen.Engine
{
    internal class MessageEngineSSCompletedCommand
    {
        public MessageEngineSSCompletedCommand(Int64 solarSystemId)
        {
            SolarSystemId = solarSystemId;
        }

        public Int64 SolarSystemId { get; private set; }
    }
}