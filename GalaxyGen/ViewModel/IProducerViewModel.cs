using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.ViewModel
{
    public interface IProducerViewModel : INotifyPropertyChanged
    {
        Producer Model { get; set; }
        string Name { get; set; }
        Agent Owner { get; set; }
        Planet Planet { get; set; }
    }

    public interface IProducerViewModelFactory
    {
        IProducerViewModel CreateProducerViewModel();
    }
}
