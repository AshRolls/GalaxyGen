using System;

namespace GalaxyGenEngine.Engine.Messages
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