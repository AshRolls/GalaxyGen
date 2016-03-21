using GalaxyGen.Model;

namespace GalaxyGen.Engine
{
    public interface IGalaxyCreator
    {
        Galaxy GetGalaxy();
        SolarSystem GetSolarSystem(string seedName);
        Planet GetPlanet(string seedName);
        Agent GetAgent(string seedName);
        Producer GetProducer(string seedName, ResourceType resProduced);
    }
}