using Akka.Actor;
using GCEngine.Engine;
using GCEngine.Model;
using GalaxyGenCore;
using GalaxyGenCore.BluePrints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.ViewModel
{
    public interface IProducerViewModel : INotifyPropertyChanged
    {
        Producer Model { get; set; }
        string Name { get; set; }
        Agent Owner { get; set; }
        Planet Planet { get; set; }
        BluePrintEnum BluePrintType { get; set; }
        Int64 TickForNextProduction { get; set; }
        bool Producing { get; set; }
        bool AutoResumeProduction { get; set; }
        int ProduceNThenStop { get; set; }
    }

    public interface IProducerViewModelFactory
    {
        IProducerViewModel CreateProducerViewModel();
    }
}
