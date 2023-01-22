using GalaxyGenEngine.Framework;
using GalaxyGenEngine.Model;
using System;
using System.ComponentModel;

namespace GalaxyGenEngine.ViewModel
{
    public interface IShipViewModel : INotifyPropertyChanged
    {
        Ship Model { get; set; }
        String Name { get; }
        Vector2 Position { get; }
    }

    public interface IShipViewModelFactory
    {
        IShipViewModel CreateShipViewModel();
    }
}
