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
    public interface IAgentViewModel : INotifyPropertyChanged
    {
        Agent Model { get; set; }
        IActorRef Actor { get; }
        string Name { get; set; }
        ISolarSystemViewModel SolarSystem { get; }
        ObservableCollection<IProducerViewModel> Producers { get; }
    }

    public interface IAgentViewModelFactory
    {
        IAgentViewModel CreateAgentViewModel();
    }
}
