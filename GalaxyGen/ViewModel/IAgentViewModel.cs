using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.ViewModel
{
    public interface IAgentViewModel : INotifyPropertyChanged
    {
        Agent Model { get; set; }
        string Name { get; set; }
        SolarSystem ss { get; }
    }

    public interface IAgentViewModelFactory
    {
        IAgentViewModel CreateAgentViewModel();
    }
}
