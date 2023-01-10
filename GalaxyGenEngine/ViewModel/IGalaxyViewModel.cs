using Akka.Actor;
using GalaxyGenEngine.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.ViewModel
{
    public interface IGalaxyViewModel : INotifyPropertyChanged
    {
        Galaxy Model { get; set; }
        IActorRef Actor { get; }
        String Name { get; set; }
        Int64 CurrentTick { get; }
        Int64 TicksPerSecond { get; }
        ICollection<ScSolarSystem> ScSolarSystems { get; }
    }

    public interface IGalaxyViewModelFactory
    {
        IGalaxyViewModel CreateGalaxyViewModel();
    }
}
