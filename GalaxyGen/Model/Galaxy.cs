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
    public class Galaxy : ModelNotifyBase
    {
        public Galaxy()
        {
            SolarSystems = new HashSet<SolarSystem>();
        }

        [Key]
        public Int64 GalaxyId { get; set; }

        [StringLength(60)]
        public String Name { get; set; }

        private Int64 currentTick_Var;
        public Int64 CurrentTick {
            get
            {
                return currentTick_Var;
            }
            set
            {
                currentTick_Var = value;
                OnPropertyChanged("CurrentTick");
            }
        }

        public virtual ICollection<SolarSystem> SolarSystems { get; set; }

        [NotMapped]
        public IActorRef Actor { get; set; }
    }
}
