using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Society
    {
        [Key]
        public Int64 SocietyId { get; set; }

        [Required]
        public String Name { get; set; }
        
        public Int64 PlanetId { get; set; }
        [ForeignKey("PlanetId")]
        public Planet Planet { get; set; }
    }
}
