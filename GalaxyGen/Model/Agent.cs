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
    public class Agent
    {
        public Agent()
        {
            Producers = new HashSet<Producer>();
            Stores = new HashSet<Store>();
            ShipsOwned = new HashSet<Ship>();
            AgentId = IdUtils.GetId();
        }

        public Int64 AgentId { get; set; }

        public String Name { get; set; }
        
        public ICollection<Producer> Producers { get; set; }
        public ICollection<Store> Stores { get; set; }
        public ICollection<Ship> ShipsOwned { get; set; }

        public IAgentLocation Location { get; set; }
        public SolarSystem SolarSystem { get; set; }

        [JsonIgnore]
        public IActorRef Actor { get; set; }
    }
}
