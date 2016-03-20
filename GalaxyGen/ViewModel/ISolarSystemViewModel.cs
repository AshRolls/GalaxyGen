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
    public interface ISolarSystemViewModel : INotifyPropertyChanged
    {
        SolarSystem Model { get; set; }
        String Name { get; set; }
        ObservableCollection<IPlanetViewModel> Planets { get; }
        ObservableCollection<IAgentViewModel> Agents { get; }
    }

    public interface ISolarSystemViewModelFactory
    {
        ISolarSystemViewModel CreateSolarSystemViewModel();
    }
}
