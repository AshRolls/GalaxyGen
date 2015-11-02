using GalaxyGen.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.ViewModel
{
    public class MainViewModel : IMainViewModel
    {
        IGalaxyCreator _galaxyCreator;
        
        public MainViewModel(IGalaxyCreator initGalaxyCreator)
        {
            _galaxyCreator = initGalaxyCreator;
        }
    }
}
