using Akka.Actor;
using GalaxyGen.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
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
