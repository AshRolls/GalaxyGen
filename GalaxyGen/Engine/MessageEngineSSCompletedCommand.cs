using System;

namespace GalaxyGen.Engine
{
    internal class MessageEngineSSCompletedCommand
    {
        public MessageEngineSSCompletedCommand(Int64 solarSystemId, Int64 tick)
        {
            SolarSystemId = solarSystemId;
            Tick = tick;
        }

        public Int64 SolarSystemId { get; private set; }
        public Int64 Tick { get; private set; }
    }
}