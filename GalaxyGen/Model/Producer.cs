using GalaxyGen.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Producer
    {
        [Key]
        public Int64 ProducerId { get; set; }

        public String Name { get; set; }

        public BluePrintEnum BluePrintType {get; set;}       
        public int TicksCompleted { get; set; }
        //public bool Producing { get; set; }

        public virtual Agent Owner { get; set; }

        public Int64 PlanetId { get; set; }
        [ForeignKey("PlanetId")]
        public Planet Planet { get; set; }

        
    }
}
