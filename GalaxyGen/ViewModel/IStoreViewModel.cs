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
    public interface IStoreViewModel : INotifyPropertyChanged
    {
        Store Model { get; set; }
        string Name { get; set; }
        Agent Owner { get; set; }
        ObservableCollection<IResourceQuantityViewModel> StoredResources { get; }
    }

    public interface IStoreViewModelFactory
    {
        IStoreViewModel CreateStoreViewModel();
    }
}
