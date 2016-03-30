using Akka.Actor;
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

        public String Name { get; set; }
        
        [Required]
        public virtual Planet Planet { get; set; }

        [NotMapped]
        public IActorRef Actor { get; set; }
    }
}
