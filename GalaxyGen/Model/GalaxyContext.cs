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
        public GalaxyContext() : base()
        {

        }

        public DbSet<Planet> Planets { get; set; }
    }
}
