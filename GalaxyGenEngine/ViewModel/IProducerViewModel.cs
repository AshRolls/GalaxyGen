using Akka.Actor;
using GalaxyGenEngine.Engine;
using GalaxyGenEngine.Model;
using GalaxyGenCore;
using GalaxyGenCore.BluePrints;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.ViewModel
{
    public interface IProducerViewModel : INotifyPropertyChanged
    {
        Producer Model { get; set; }
        string Name { get; set; }
        ulong OwnerId { get; set; }
        BluePrintEnum BluePrintType { get; set; }
        UInt64 TickForNextProduction { get; set; }
        bool Producing { get; set; }
        bool AutoResumeProduction { get; set; }
        int ProduceNThenStop { get; set; }
    }

    public interface IProducerViewModelFactory
    {
        IProducerViewModel CreateProducerViewModel();
    }
}
