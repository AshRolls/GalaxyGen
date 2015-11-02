using System;

namespace GalaxyGen.Model
{
    public interface IPlanet
    {
        Int64 Population { get; set; }
        ISociety Society { get; set; }
        IMarket Market { get; set; }        
    }
}