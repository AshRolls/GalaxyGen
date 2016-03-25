using Akka.Actor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public abstract class ModelActor
    {
        [NotMapped]
        public IActorRef Actor { get; set; }
    }
}
