using Akka.Actor;
using GCEngine.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GCEngine.Model
{
    public class Galaxy
    {
        public Galaxy()
        {
            SolarSystems = new HashSet<SolarSystem>();
            ShipTypes = new HashSet<ShipType>();
            GalaxyId = IdUtils.GetId();
        }

        public Int64 GalaxyId { get; set; }

        public String Name { get; set; }
        
        public Int64 CurrentTick { get; set; }

        public Int64 MaxId { get; set; }

        public ICollection<SolarSystem> SolarSystems { get; set; }
        public ICollection<ShipType> ShipTypes { get; set; }

        [JsonIgnore]
        public Int64 TicksPerSecond { get; set; }
        [JsonIgnore]
        public IActorRef Actor { get; set; }
    }
}
