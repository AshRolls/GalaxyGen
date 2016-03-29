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
        public Ship()
        {
            StoredResources = new HashSet<ResourceQuantity>();
        }

        [Key]
        public Int64 ShipId { get; set; }
      
        [StringLength(60)]
        public String Name { get; set; }

        public virtual Agent Owner { get; set; }

        public virtual ICollection<ResourceQuantity> StoredResources { get; set; }        

        public Int64 SolarSystemId { get; set; }
        [ForeignKey("SolarSystemId")]
        public SolarSystem SolarSystem { get; set; }

        [NotMapped]
        public IActorRef Actor { get; set; }
    }
}
