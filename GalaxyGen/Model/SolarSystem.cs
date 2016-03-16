using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class SolarSystem
    {
        [Key]
        public Int64 SolarSystemId { get; set; }

        [Required]
        [StringLength(60)]
        public String Name { get; set; }

        [Required]
        public virtual ICollection<Planet> Planets { get; set; }
    }
}
