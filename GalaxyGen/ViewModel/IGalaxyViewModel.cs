using Akka.Actor;
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
        IActorRef Actor { get; }
        String Name { get; set; }
        Int64 CurrentTick { get; }
        Int64 TicksPerSecond { get; }
        ObservableCollection<ISolarSystemViewModel> SolarSystems { get; }
        ObservableCollection<IResourceTypeViewModel> ResourceTypes { get; }
    }

    public interface IGalaxyViewModelFactory
    {
        IGalaxyViewModel CreateGalaxyViewModel();
    }
}
