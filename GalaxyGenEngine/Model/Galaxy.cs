using Akka.Actor;
using GalaxyGenEngine.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GalaxyGenEngine.Model
{
    public class Galaxy
    {
        public Galaxy()
        {
            SolarSystems = new HashSet<SolarSystem>();
            ShipTypes = new HashSet<ShipType>();
            Accounts = new();
            Currencies = new();
            GalaxyId = IdUtils.GetId();
        }

        public UInt64 GalaxyId { get; set; }

        public String Name { get; set; }
        
        public UInt64 CurrentTick { get; set; }

        public UInt64 MaxId { get; set; }

        public ICollection<SolarSystem> SolarSystems { get; set; }
        public ICollection<ShipType> ShipTypes { get; set; }
        public Dictionary<ulong, Account> Accounts { get; set; }
        public Dictionary<ulong, Currency> Currencies { get; set; }

        [JsonIgnore]
        public UInt64 TicksPerSecond { get; set; }
        [JsonIgnore]
        public IActorRef Actor { get; set; }
    }
}
