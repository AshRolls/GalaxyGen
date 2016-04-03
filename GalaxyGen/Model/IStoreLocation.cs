using System;
using System.Collections.Generic;

namespace GalaxyGen.Model
{
    public interface IStoreLocation
    {
        Dictionary<Int64,Store> Stores { get; set; }
    }
}