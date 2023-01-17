using GalaxyGenEngine.Framework;
using System;
using GalaxyGenCore.BluePrints;

namespace GalaxyGenEngine.Model
{
    public class Producer
    {
        public Producer()
        {
            ProducerId = IdUtils.GetId();
        }

        public UInt64 ProducerId { get; set; }

        public String Name { get; set; }

        public BluePrintEnum BluePrintType {get; set;}       
        public UInt64 TickForNextProduction { get; set; }
        public bool Producing { get; set; }
        public bool AutoResumeProduction { get; set; }
        public int ProduceNThenStop { get; set; }

        public ulong OwnerId { get; set; }

        public ulong PlanetScId { get; set; }
    }
}
