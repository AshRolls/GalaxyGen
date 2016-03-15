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
        Planet _planet;

        public TickEngine(Planet initPlanet)
        {
            _planet = initPlanet;
        }

        public void RunTick(int numberOfTicks)
        {
            throw new NotImplementedException();
        }
    }
}
