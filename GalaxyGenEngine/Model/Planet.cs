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
            Stores = new Dictionary<Int64, Store>();
            DockedShips = new Dictionary<long, Ship>();
            Agents = new List<Agent>();            
        }

        public String Name { get; set; }
        public Int64 PlanetId { get; set; }
        public Int64 StarChartId { get; set; }
        public TypeEnum GalType { get; set; }
               
        public Double PositionX { get; set; }
        public Double PositionY { get; set; }

        public Int64 Population { get; set; }

        public Society Society { get; set; }

        public ICollection<Producer> Producers { get; set; }
        public Dictionary<long, Store> Stores { get; set; }
        public Dictionary<long, Ship> DockedShips { get; set; }
        public ICollection<Agent> Agents { get; set; }
        public SolarSystem SolarSystem { get; set; }
        public Market Market { get; set; }

    }
}
