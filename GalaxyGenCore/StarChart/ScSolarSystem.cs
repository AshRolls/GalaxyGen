using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenCore.StarChart
{
    public class ScSolarSystem
    {
        public string Name { get; set; }
        public ICollection<ScPlanet> Planets { get; set; }
    }
}
