using GalaxyGen.Framework;
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
            StoredResources = new HashSet<ResourceQuantity>();            
        }

        public Int64 StoreId { get; set; }

        public String Name { get; set; }

        public ICollection<ResourceQuantity> StoredResources { get; set; }
        public Agent Owner { get; set; }

        public IStoreLocation Location { get; set; }


    }
}
