using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Store : ModelActor
    {
        [Key]
        public Int64 StoreId { get; set; }

        public String Name { get; set; }

        public virtual ICollection<ResourceQuantity> StoredResources { get; set; }

        public virtual Agent Owner { get; set; }
        public virtual Planet Location { get; set; }                
    }
}
