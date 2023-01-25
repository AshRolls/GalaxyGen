﻿using GalaxyGenEngine.Framework;
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
        public ulong TickForNextProduction { get; set; }        

        public bool Producing { get; set; }
        public bool AutoResumeProduction { get; set; }
        public bool AutoBuyFromMarket { get; set; } 
        public bool AutoBuyFailed { get; set; }
        public bool AutoSellToMarket { get; set; }
        public int ProduceNThenStop { get; set; }

        public ulong OwnerId { get; set; }

        public ulong PlanetScId { get; set; }
    }
}
