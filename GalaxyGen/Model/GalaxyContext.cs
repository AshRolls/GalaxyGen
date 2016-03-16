using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class GalaxyContext : DbContext
    {
        static GalaxyContext()
        {
            Database.SetInitializer<GalaxyContext>(new GalaxyInitialiser());
            using (GalaxyContext db = new GalaxyContext())
            {
                db.Database.Initialize(false);
            }
        }

        public DbSet<SolarSystem> SolarSystems { get; set; }        
    }
}
