using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class PlanetController
    {
        private Planet _model;
        private HashSet<ProducerController> _producerCs;

        public PlanetController(Planet p)
        {
            _model = p;
            _producerCs = new HashSet<ProducerController>();
            // create child controllers for each producer in planet
            foreach (Producer prod in p.Producers)
            {
                ProducerController pc = new ProducerController(prod);
                _producerCs.Add(pc);
            }

        }

        public void Tick(MessageTick tick)
        {
            
        }


    }
}
