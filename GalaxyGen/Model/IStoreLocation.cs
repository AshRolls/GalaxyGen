using System.Collections.Generic;

namespace GalaxyGen.Model
{
    public interface IStoreLocation
    {
        ICollection<Store> Stores { get; set; }
    }
}