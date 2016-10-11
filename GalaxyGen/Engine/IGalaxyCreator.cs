using GalaxyGen.Model;
using GalaxyGenCore;
using System.Collections.Generic;

namespace GalaxyGen.Engine
{
    public interface IGalaxyCreator
    {
        Galaxy GetFullGalaxy();
        Galaxy GetGalaxy();
        SolarSystem GetSolarSystem(string seedName);
        Planet GetPlanet(string seedName, double orbitalKm, double orbitalDays);
        Agent GetAgent(string seedName);
        Producer GetProducer(string seedName, BluePrintEnum bpType);
    }
}