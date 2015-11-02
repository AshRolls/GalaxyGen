using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class TickEngine : ITickEngine
    {
        IPlanet _planet;

        public TickEngine(IPlanet initPlanet)
        {
            _planet = initPlanet;
        }

        public void RunTick(int numberOfTicks)
        {
            throw new NotImplementedException();
        }
    }
}
