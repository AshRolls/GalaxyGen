﻿using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.ViewModel
{
    public interface IPlanetViewModel : INotifyPropertyChanged
    {
        Planet Model { get; set; }
        String Name { get; set; }
        Int64 Population { get; set; }
        Society Society { get; }
    }

    public interface IPlanetViewModelFactory
    {
        IPlanetViewModel CreatePlanetViewModel();
    }
}
