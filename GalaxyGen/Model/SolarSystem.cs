using Akka.Actor;
using GalaxyGen.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class SolarSystem
    {
        public SolarSystem()
        {
            SolarSystemId = IdUtils.GetId();
            Planets = new List<Planet>();
            Agents = new List<Agent>();
            Ships = new HashSet<Ship>();
        }

        public Int64 SolarSystemId { get; set; }
        public Int64 StarChartId { get; set; }

        public ICollection<Planet> Planets { get; set; }
        public ICollection<Agent> Agents { get; set; }
        public ICollection<Ship> Ships { get; set; }

        [JsonIgnore]
        public String Name { get; set; }

        public virtual Galaxy Galaxy { get; set; }

        [JsonIgnore]
        public IActorRef Actor { get; set; }
    }
}
