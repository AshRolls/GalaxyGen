using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class ProducerController
    {
        private Producer _model;        

        public ProducerController(Producer p)
        {
            _model = p;
        }

        public void Tick(MessageTick tick)
        {
            
        }


    }
}
