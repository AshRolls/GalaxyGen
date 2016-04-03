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
    public struct ResourceQuantity
    {
        public ResourceTypeEnum Type { get; set; }
        public UInt64 Quantity { get; set; }
    }
}
