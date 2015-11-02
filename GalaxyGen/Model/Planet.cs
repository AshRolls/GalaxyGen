using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Planet : IPlanet
    {
        ISociety _society;
        IMarket _market;

        Int64 _population;

        public Planet()
        {

        }

        public ISociety Society
        {
            get { return _society; }
            set { _society = value; }
        }

        public IMarket Market
        {
            get { return _market; }
            set { _market = value; }
        }

        public Int64 Population
        {
            get { return _population; }
            set { _population = value; }
        }

    }
}
