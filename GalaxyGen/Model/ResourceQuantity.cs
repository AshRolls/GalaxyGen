using GalaxyGen.Engine;
using GalaxyGen.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class ResourceQuantity
    {
        public ResourceQuantity()
        {
             ResourceQuantityId = IdUtils.GetId();
        }

        public Int64 ResourceQuantityId { get; set;}
        public ResourceTypeEnum Type { get; set; }
        public Int64 Quantity { get; set; }
    }
}
