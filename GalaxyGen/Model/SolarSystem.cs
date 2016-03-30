using Akka.Actor;
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
            Planets = new List<Planet>();
            Agents = new List<Agent>();
            Ships = new HashSet<Ship>();
        }

        [Key]
        public Int64 SolarSystemId { get; set; }

        [StringLength(60)]
        public String Name { get; set; }

        public virtual ICollection<Planet> Planets { get; set; }
        public virtual ICollection<Agent> Agents { get; set; }
        public virtual ICollection<Ship> Ships { get; set; }
        public virtual Galaxy Galaxy { get; set; }

        [NotMapped]
        public IActorRef Actor { get; set; }
    }
}
