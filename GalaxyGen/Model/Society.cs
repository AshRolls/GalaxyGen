using Akka.Actor;
using GalaxyGen.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Society
    {
        public Society()
        {
            SocietyId = IdUtils.GetId();
        }

        public Int64 SocietyId { get; set; }

        public String Name { get; set; }
        
        public virtual Planet Planet { get; set; }
    }
}
