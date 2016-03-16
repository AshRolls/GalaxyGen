using GalaxyGen.Model;

namespace GalaxyGen.Engine
{
    public interface IGalaxyCreator
    {
        SolarSystem GetSolarSystem(string seedName);
        Planet GetPlanet(string seedName);
    }
}