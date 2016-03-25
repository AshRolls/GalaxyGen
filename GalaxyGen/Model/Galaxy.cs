using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Galaxy : ModelActor
    {
        [Key]
        public Int64 GalaxyId { get; set; }

        [StringLength(60)]
        public String Name { get; set; }
        public Int64 CurrentTick { get; set; }

        public virtual ICollection<SolarSystem> SolarSystems { get; set; }
    }
}
