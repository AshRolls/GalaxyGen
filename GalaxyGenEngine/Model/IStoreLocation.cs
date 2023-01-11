using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{
    public interface IStoreLocation
    {
        Dictionary<UInt64,Store> Stores { get; set; }
    }
}