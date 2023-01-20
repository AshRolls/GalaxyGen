using GalaxyGenEngine.Engine;
using GalaxyGenEngine.Model;
using GalaxyGenCore;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.ViewModel
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
