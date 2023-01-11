using System;

namespace GalaxyGenEngine.Engine.Messages
{
    internal class MessageEngineSSCompletedCommand
    {
        public MessageEngineSSCompletedCommand(UInt64 solarSystemId)
        {
            SolarSystemId = solarSystemId;            
        }

        public UInt64 SolarSystemId { get; private set; }        
    }
}