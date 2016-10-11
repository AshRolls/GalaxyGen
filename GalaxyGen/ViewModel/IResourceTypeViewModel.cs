using GalaxyGen.Engine;
using GalaxyGen.Model;
using GalaxyGenCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.ViewModel
{
    public interface IResourceTypeViewModel : INotifyPropertyChanged
    {
        ResourceType Model { get; set; }
        ResourceTypeEnum Type { get; }
        String Name { get; }
        Double VolumePerUnit { get; }
        Int64 DefaultTicksToProduce { get; }
    }

    public interface IResourceTypeViewModelFactory
    {
        IResourceTypeViewModel CreateResourceTypeViewModel();
    }
}
