using Akka.Actor;
using GalaxyGenEngine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.ViewModel
{
    public interface IShipViewModel : INotifyPropertyChanged
    {
        Ship Model { get; set; }
        String Name { get; }
        Double PositionX { get; }
        Double PositionY { get; }
    }

    public interface IShipViewModelFactory
    {
        IShipViewModel CreateShipViewModel();
    }
}
