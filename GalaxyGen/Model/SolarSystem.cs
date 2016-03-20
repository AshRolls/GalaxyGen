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
        [Key]
        public Int64 SolarSystemId { get; set; }

        [StringLength(60)]
        public String Name { get; set; }

        public virtual ICollection<Planet> Planets { get; set; }
        public virtual ICollection<Agent> Agents { get; set; }

        public Int64 GalaxyId { get; set; }
        [ForeignKey("GalaxyId")]
        public Galaxy Galaxy { get; set; }
    }
}
