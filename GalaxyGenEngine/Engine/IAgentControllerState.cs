using System.Collections.Generic;
using GCEngine.Framework;
using GCEngine.Model;
using GalaxyGenCore.StarChart;
using GalaxyGenCore.Resources;
using System;

namespace GCEngine.Engine
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
        UInt64 CurrentShipResourceQuantity(ResourceTypeEnum resType);
        double DestinationX(long destinationScId);
        double DestinationY(long destinationScId);
        bool XYAtDestination(long destinationScId, double X, double Y);
        UInt64 PlanetResourceQuantity(Int64 planetScId, ResourceTypeEnum res);
        List<ResourceQuantity> PlanetResources(Int64 planetScId);
        bool TryGetPlanetStoreId(Int64 planetScId, out long storeId);
    }
}