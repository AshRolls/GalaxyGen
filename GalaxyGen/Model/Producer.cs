using Akka.Actor;
using GalaxyGen.Engine;
using GalaxyGen.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGenCore;
using GalaxyGenCore.BluePrints;

namespace GalaxyGen.Model
{
    public class Producer
    {
        public Producer()
        {
            ProducerId = IdUtils.GetId();
        }

        public Int64 ProducerId { get; set; }

        public String Name { get; set; }

        public BluePrintEnum BluePrintType {get; set;}       
        public Int64 TickForNextProduction { get; set; }
        public bool Producing { get; set; }
        public bool AutoResumeProduction { get; set; }
        public int ProduceNThenStop { get; set; }

        public virtual Agent Owner { get; set; }

        public virtual Planet Planet { get; set; }
    }
}
