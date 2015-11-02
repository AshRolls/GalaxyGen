using GalaxyGen.Model;
using System.Collections.ObjectModel;

namespace GalaxyGen.ViewModel
{
    public interface IMainViewModel
    {
        ObservableCollection<IPlanetViewModel> Planets { get; set; }
    }
}