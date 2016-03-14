using GalaxyGen.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GalaxyGen.ViewModel
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        ObservableCollection<IPlanetViewModel> Planets { get; set; }
    }
}