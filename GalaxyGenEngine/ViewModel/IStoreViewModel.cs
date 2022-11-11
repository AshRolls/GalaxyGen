using GCEngine.Engine;
using GCEngine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.ViewModel
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
