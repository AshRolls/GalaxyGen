using GalaxyGen.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class ResourceType
    {
        [Key]
        public Int64 ResourceTypeId { get; set; }

        public ResourceTypeEnum Type { get; set; }
        public String Name { get; set; }
        public Double VolumePerUnit { get; set; }
        public Int64 DefaultTicksToProduce { get; set; }

        public virtual ICollection<Producer> Producers { get; set; }
    }
}
