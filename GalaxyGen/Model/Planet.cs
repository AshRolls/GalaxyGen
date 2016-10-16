using Akka.Actor;
using GalaxyGen.Framework;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{   
    public class Planet : IAgentLocation, IStoreLocation
    {
        public Planet(String name, Double orbitKm, Double orbitDays)
        {
            Name = name;
            OrbitKm = orbitKm;
            OrbitDays = orbitDays;
            PlanetId = IdUtils.GetId();
            GalType = TypeEnum.Planet;
            Producers = new HashSet<Producer>();
            Stores = new Dictionary<Int64,Store>();
            DockedShips = new List<Ship>();
            Agents = new List<Agent>();
        }

        [JsonIgnore]
        public String Name { get; set; }

        public Int64 PlanetId { get; set; }
        public Int64 StarChartId { get; set; }
        public TypeEnum GalType { get; set; }

        [JsonIgnore]
        public Double OrbitKm { get; set; }
        [JsonIgnore]
        public Double OrbitDays { get; set; }
               
        public Double PositionX { get; set; }
        public Double PositionY { get; set; }

        public Int64 Population { get; set; }

        public Society Society { get; set; }

        public ICollection<Producer> Producers { get; set; }
        public Dictionary<Int64,Store> Stores { get; set; }
        public ICollection<Ship> DockedShips { get; set; }
        public ICollection<Agent> Agents { get; set; }
        public SolarSystem SolarSystem { get; set; }

        //public IMarket Market { get; set; }

    }
}
