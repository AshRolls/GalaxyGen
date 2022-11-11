using System;
using System.Collections.Generic;

namespace GCEngine.Model
{
    public interface IStoreLocation
    {
        Dictionary<Int64,Store> Stores { get; set; }
    }
}