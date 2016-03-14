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
    public class Planet
    {
        [Key]
        public Int64 PlanetId { get; set; }
      
        [Required]
        [StringLength(30)]
        public String Name { get; set; }

        [Required]
        public Int64 Population { get; set; }
        
        [Required]
        public virtual Society Soc { get; set; }

        //[ForeignKey("MarketId")]
        //public IMarket Market { get; set; }


    }
}
