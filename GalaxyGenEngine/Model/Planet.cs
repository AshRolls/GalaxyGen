using GalaxyGenEngine.Framework;
using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{   
    public class Planet : IAgentLocation, IStoreLocation
    {
        public Planet()
        {
            PlanetId = IdUtils.GetId();
            GalType = TypeEnum.Planet;
            Producers = new HashSet<Producer>();
            Stores = new Dictionary<ulong, Store>();
            DockedShips = new Dictionary<ulong, Ship>();
            Agents = new List<Agent>();            
        }

        public String Name { get; set; }
        public UInt64 PlanetId { get; set; }
        public UInt64 StarChartId { get; set; }
        public TypeEnum GalType { get; set; }

        public Vector2 Position { get; set; }

        public Int64 Population { get; set; }

        public Society Society { get; set; }

        public ICollection<Producer> Producers { get; set; }
        public Dictionary<ulong, Store> Stores { get; set; }
        public Dictionary<ulong, Ship> DockedShips { get; set; }
        public ICollection<Agent> Agents { get; set; }
        public SolarSystem SolarSystem { get; set; }
        public Market Market { get; set; }

    }
}
