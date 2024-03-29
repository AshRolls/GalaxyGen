﻿using Akka.Actor;
using GalaxyGenEngine.Framework;
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
    public interface IPlanetViewModel : INotifyPropertyChanged
    {
        Planet Model { get; set; }
        String Name { get; }
        //Int64 Population { get; set; }
        Vector2 Position { get; }
        ISocietyViewModel Society { get; }
        IMarketViewModel Market { get; }
        ObservableCollection<IProducerViewModel> Producers { get; }
    }

    public interface IPlanetViewModelFactory
    {
        IPlanetViewModel CreatePlanetViewModel();
    }
}
