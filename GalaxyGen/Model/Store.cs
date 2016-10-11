using GalaxyGen.Engine;
using GalaxyGen.Framework;
using GalaxyGenCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Store
    {
        public Store()
        {
            StoreId = IdUtils.GetId();
            StoredResources = new Dictionary<ResourceTypeEnum, UInt64>();
        }

        public Int64 StoreId { get; set; }

        public String Name { get; set; }

        public Dictionary<ResourceTypeEnum,UInt64> StoredResources { get; set; }
        public Agent Owner { get; set; }

        public IStoreLocation Location { get; set; }


    }
}
