using Akka.Actor;
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
    public class Ship
    {
        [Key]
        public Int64 ShipId { get; set; }
      
        [StringLength(60)]
        public String Name { get; set; }

        public virtual Agent Owner { get; set; }
        
        [Required]
        public virtual Store Store { get; set; }

        public virtual SolarSystem SolarSystem { get; set; }

        [NotMapped]
        public IActorRef Actor { get; set; }
    }
}
