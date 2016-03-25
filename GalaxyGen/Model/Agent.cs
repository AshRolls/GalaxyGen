using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Agent : ModelActor
    {
        [Key]
        public Int64 AgentId { get; set; }

        public String Name { get; set; }

        public virtual ICollection<Producer> Producers { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        
        public Int64 SolarSystemId { get; set; }
        [ForeignKey("SolarSystemId")]
        public SolarSystem SolarSystem { get; set; }
    }
}
