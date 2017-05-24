using System.Collections.Generic;
using GalaxyGen.Framework;
using GalaxyGen.Model;
using GalaxyGenCore.StarChart;

namespace GalaxyGen.Engine
{
    public interface IAgentControllerState
    {
        bool CurrentShipAutopilotActive { get; }
        double CurrentShipCruisingSpeed { get; }
        long CurrentShipDockedPlanetScId { get; }
        bool CurrentShipHasDestination { get; }
        long CurrentShipId { get; }
        bool CurrentShipIsDocked { get; }
        ShipStateEnum CurrentShipState { get; }
        double CurrentShipX { get; }
        PointD CurrentShipXY { get; }
        double CurrentShipY { get; }
        bool IsPilotingShip { get; }
        string Memory { get; set; }
        IEnumerable<ScPlanet> PlanetsInSolarSystem { get; }
        IEnumerable<long> PlanetsInSolarSystemScIds { get; }

        bool CurrentShipAtDestination(long destinationScId);
        double DestinationX(long destinationScId);
        double DestinationY(long destinationScId);
        bool XYAtDestination(long destinationScId, double X, double Y);
    }
}