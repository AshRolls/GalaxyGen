using System;

namespace GalaxyGen.Model
{
    public interface IPlanet
    {
        Int64 Id { get; set; }
        String Name { get; set; }
        Int64 Population { get; set; }
        ISociety Society { get; set; }
        IMarket Market { get; set; }        
    }
}