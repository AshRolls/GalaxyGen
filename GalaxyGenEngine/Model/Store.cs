using GalaxyGenEngine.Framework;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
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
