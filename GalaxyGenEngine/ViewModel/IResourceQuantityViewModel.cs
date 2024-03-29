﻿using GalaxyGenEngine.Engine;
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
    public interface IResourceQuantityViewModel : INotifyPropertyChanged
    {
        ResourceTypeEnum Type { get; set; }
        Int64 Quantity { get; set; }
    }

    public interface IResourceQuantityViewModelFactory
    {
        IResourceQuantityViewModel CreateResourceQuantityViewModel();
    }
}
