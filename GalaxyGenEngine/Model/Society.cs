using GalaxyGenEngine.Framework;
using System;

namespace GalaxyGenEngine.Model
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
