using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Planet : ModelActor
    {
        [Key]
        public Int64 PlanetId { get; set; }
      
        [StringLength(60)]
        public String Name { get; set; }
        public Int64 Population { get; set; }
        
        [Required]
        public virtual Society Society { get; set; }

        public virtual ICollection<Producer> Producers { get; set; }
        public virtual ICollection<Store> Stores { get; set; }

        public Int64 SolarSystemId { get; set; }
        [ForeignKey("SolarSystemId")]
        public SolarSystem SolarSystem { get; set; }

        //[ForeignKey("MarketId")]
        //public IMarket Market { get; set; }

    }
}
