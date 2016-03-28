using GalaxyGen.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace GalaxyGen.ViewModel
{
    public interface IMainGalaxyViewModel : INotifyPropertyChanged
    {
        IGalaxyViewModel Galaxy { get; set; }
        ISolarSystemViewModel SelectedSolarSystem { get; set; }
        IPlanetViewModel SelectedPlanet { get; set; }
        IAgentViewModel SelectedAgent { get; set; }
        ITextOutputViewModel TextOutput { get; set; }
        ICommand WindowClosing { get; }
        ICommand RunEngineCommand { get; }
    }
}