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
            StoredResources = new Dictionary<ResourceTypeEnum, Int64>();
        }

        public UInt64 StoreId { get; set; }

        public String Name { get; set; }

        public Dictionary<ResourceTypeEnum,Int64> StoredResources { get; set; }
        public Agent Owner { get; set; }

        public IStoreLocation Location { get; set; }


    }
}
