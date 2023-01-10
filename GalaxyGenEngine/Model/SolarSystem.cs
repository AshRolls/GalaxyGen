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
            Planets = new Dictionary<long, Planet>();
            Agents = new List<Agent>();
            Ships = new Dictionary<long, Ship>();
        }

        public Int64 SolarSystemId { get; set; }
        public Int64 StarChartId { get; set; }

        public Dictionary<long, Planet> Planets { get; set; }
        public ICollection<Agent> Agents { get; set; }
        public Dictionary<long, Ship> Ships { get; set; }

        [JsonIgnore]
        public String Name { get; set; }

        public virtual Galaxy Galaxy { get; set; }

        [JsonIgnore]
        public IActorRef Actor { get; set; }
    }
}
