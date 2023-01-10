using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{
    public interface IStoreLocation
    {
        Dictionary<Int64,Store> Stores { get; set; }
    }
}