using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.ViewModel
{
    public interface IGalaxyViewModel : INotifyPropertyChanged
    {
        Galaxy Model { get; set; }
        String Name { get; set; }
        ObservableCollection<ISolarSystemViewModel> SolarSystems { get; }
    }

    public interface IGalaxyViewModelFactory
    {
        IGalaxyViewModel CreateGalaxyViewModel();
    }
}
