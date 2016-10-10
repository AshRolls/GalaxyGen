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
    public interface IPlanetViewModel : INotifyPropertyChanged
    {
        Planet Model { get; set; }
        String Name { get; set; }
        Int64 Population { get; set; }
        Double PositionX { get; }
        Double PositionY { get; }
        int PosX300 { get; }
        int PosY300 { get; }
        ISocietyViewModel Society { get; }
        ObservableCollection<IProducerViewModel> Producers { get; }
    }

    public interface IPlanetViewModelFactory
    {
        IPlanetViewModel CreatePlanetViewModel();
    }
}
