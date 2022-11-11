using GCEngine.Model;
using GalaxyGenCore.StarChart;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace GCEngine.ViewModel
{
    public interface IMainGalaxyViewModel : INotifyPropertyChanged
    {
        IGalaxyViewModel Galaxy { get; set; }
        ISolarSystemViewModel SelectedSolarSystemVm { get; }
        ScSolarSystem SelectedScSolarSystem { get; set; }
        IPlanetViewModel SelectedPlanetVm { get; }
        ScPlanet SelectedScPlanet { get; set; }
        IAgentViewModel SelectedAgent { get; set; }
        ITextOutputViewModel TextOutput { get; set; }
        ICommand WindowClosing { get; }
        ICommand RunEngineCommand { get; }
        ICommand RunMaxEngineCommand { get; }
        ICommand StopEngineCommand { get; }
    }
}