using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Planet : IPlanet
    {
        [Key]
        public Int64 Id { get; set; }
      
        [Required]
        [StringLength(30)]
        public String Name { get; set; }

        [Required]
        public Int64 Population { get; set; }
        
        [ForeignKey("SocietyId")]
        public ISociety Society { get; set; }

        [ForeignKey("MarketId")]
        public IMarket Market { get; set; }


    }
}
