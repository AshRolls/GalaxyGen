using GalaxyGenEngine.Framework;
using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{
    public enum ShipStateEnum
    {
        Unpiloted,
        Docked,        
        SpaceManual,
        SpaceAutopilot
    }

    public class Ship : IAgentLocation, IStoreLocation
    {
        public Ship()
        {
            ShipId = IdUtils.GetId();
            GalType = TypeEnum.Ship;
            Agents = new List<Agent>();
            Stores = new Dictionary<UInt64, Store>();
        }

        public UInt64 ShipId { get; set; }
        public TypeEnum GalType { get; set; }

        public String Name { get; set; }
        public ShipType Type { get; set; }

        public ShipStateEnum ShipState { get; set; }
       
        public Vector2 Position { get; set; }
        public Agent Owner { get; set; }
        public Agent Pilot { get; set; }
        public Dictionary<UInt64, Store> Stores { get; set; }
        public SolarSystem SolarSystem { get; set; }
        public Planet DockedPlanet { get; set; }

        public ICollection<Agent> Agents { get; set; }

        public UInt64 DestinationScId { get; set; }

        public bool AutopilotActive { get; set; }
    }
}
