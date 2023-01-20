using System.Collections.Generic;
using GalaxyGenEngine.Framework;
using GalaxyGenEngine.Model;
using GalaxyGenCore.StarChart;
using GalaxyGenCore.Resources;
using System;

namespace GalaxyGenEngine.Engine
{
    public interface IAgentControllerState
    {
        bool CurrentShipAutopilotActive { get; }
        double CurrentShipCruisingSpeed { get; }
        ulong CurrentShipDockedPlanetScId { get; }
        bool CurrentShipHasDestination { get; }
        ulong CurrentShipId { get; }
        bool CurrentShipIsDocked { get; }
        ShipStateEnum CurrentShipState { get; }
        double CurrentShipX { get; }
        PointD CurrentShipXY { get; }
        double CurrentShipY { get; }
        bool IsPilotingShip { get; }
        string Memory { get; set; }
        IEnumerable<ScPlanet> PlanetsInSolarSystem { get; }
        IEnumerable<ulong> PlanetsInSolarSystemScIds { get; }        
        bool CurrentShipAtDestination(ulong destinationScId);
        Int64 CurrentShipResourceQuantity(ResourceTypeEnum resType);
        Int64 CurrentPlanetResourceQuantity(ResourceTypeEnum resType);
        double DestinationX(ulong destinationScId);
        double DestinationY(ulong destinationScId);
        bool XYAtDestination(ulong destinationScId, double X, double Y);
        Int64 PlanetResourceQuantity(UInt64 planetScId, ResourceTypeEnum res);
        List<ResourceQuantity> PlanetResources(UInt64 planetScId);
        bool TryGetPlanetStoreId(ulong planetScId, out ulong storeId);
    }
}