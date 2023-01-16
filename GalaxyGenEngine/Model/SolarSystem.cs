using Akka.Actor;
using GalaxyGenEngine.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{
    public class SolarSystem
    {
        public SolarSystem()
        {
            SolarSystemId = IdUtils.GetId();
            Planets = new Dictionary<ulong, Planet>();
            Agents = new List<Agent>();
            Ships = new Dictionary<ulong, Ship>();
        }

        public UInt64 SolarSystemId { get; set; }
        public UInt64 StarChartId { get; set; }        
        public Dictionary<ulong, Planet> Planets { get; set; }
        public ICollection<Agent> Agents { get; set; }
        public Dictionary<ulong, Ship> Ships { get; set; }

        [JsonIgnore]
        public String Name { get; set; }

        public virtual Galaxy Galaxy { get; set; }

        [JsonIgnore]
        public IActorRef Actor { get; set; }
    }
}
