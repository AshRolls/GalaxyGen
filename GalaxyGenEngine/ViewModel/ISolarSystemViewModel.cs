using Akka.Actor;
using GCEngine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.ViewModel
{
    public interface ISolarSystemViewModel : INotifyPropertyChanged
    {
        SolarSystem Model { get; set; }
        IActorRef Actor { get; }
        String Name { get; set; }
        ObservableCollection<IPlanetViewModel> Planets { get; }
        ObservableCollection<IAgentViewModel> Agents { get; }
        ObservableCollection<IShipViewModel> Ships { get; }
    }

    public interface ISolarSystemViewModelFactory
    {
        ISolarSystemViewModel CreateSolarSystemViewModel();
    }
}
