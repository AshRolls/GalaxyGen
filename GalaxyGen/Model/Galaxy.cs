using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Galaxy
    {
        [Key]
        public Int64 GalaxyId { get; set; }

        [Required]
        [StringLength(60)]
        public String Name { get; set; }

        [Required]
        public Int64 CurrentTick { get; set; }

        [Required]
        public virtual ICollection<SolarSystem> SolarSystems { get; set; }

        [Required]
        public virtual ICollection<Agent> Agents { get; set; }
    }
}
