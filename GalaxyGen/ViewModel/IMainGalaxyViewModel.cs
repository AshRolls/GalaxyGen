using GalaxyGen.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace GalaxyGen.ViewModel
{
    public interface IMainGalaxyViewModel : INotifyPropertyChanged
    {
        ObservableCollection<IPlanetViewModel> Planets { get; set; }
        IPlanetViewModel SelectedPlanet { get; set; }
        ICommand WindowClosing { get; }
        ICommand RunEngineCommand { get; }

    }
}