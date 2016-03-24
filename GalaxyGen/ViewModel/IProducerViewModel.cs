using Akka.Actor;
using GalaxyGen.Engine;
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
    public interface IProducerViewModel : INotifyPropertyChanged
    {
        Producer Model { get; set; }
        IActorRef Actor { get; set; }
        string Name { get; set; }
        Agent Owner { get; set; }
        Planet Planet { get; set; }
        BluePrintEnum BluePrintType { get; set; }
        int TicksRemaining { get; set; }        
    }

    public interface IProducerViewModelFactory
    {
        IProducerViewModel CreateProducerViewModel();
    }
}
