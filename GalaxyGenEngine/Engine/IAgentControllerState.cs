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
        Vector2 CurrentShipPos { get; }        
        bool IsPilotingShip { get; }
        string Memory { get; set; }
        IEnumerable<ScPlanet> PlanetsInSolarSystem { get; }
        IEnumerable<ulong> PlanetsInSolarSystemScIds { get; }        
        bool CurrentShipAtDestination(ulong destinationScId);
        Int64 CurrentShipResourceQuantity(ResourceTypeEnum resType);
        Int64 CurrentPlanetResourceQuantity(ResourceTypeEnum resType);

        Vector2 Destination(ulong destinationScId);        
        bool AtDestination(ulong destinationScId, Vector2 pos);
        Int64 PlanetResourceQuantity(UInt64 planetScId, ResourceTypeEnum res);
        List<ResourceQuantity> PlanetResources(UInt64 planetScId);
        bool TryGetPlanetStoreId(ulong planetScId, out ulong storeId);
    }
}