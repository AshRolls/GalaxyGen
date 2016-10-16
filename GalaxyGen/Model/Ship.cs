using Akka.Actor;
using GalaxyGen.Framework;
using Newtonsoft.Json;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public enum ShipStateEnum
    {
        Docked,        
        SpaceCruising,
        SpaceStopped
    }

    public class Ship : IAgentLocation, IStoreLocation
    {
        public Ship()
        {
            ShipId = IdUtils.GetId();
            GalType = TypeEnum.Ship;
            Agents = new List<Agent>();
            Stores = new Dictionary<Int64, Store>();
        }

        public Int64 ShipId { get; set; }
        public TypeEnum GalType { get; set; }

        public String Name { get; set; }
        public ShipType Type { get; set; }

        public ShipStateEnum ShipState { get; set; }
        public Double PositionX { get; set; }
        public Double PositionY { get; set; }

        public Agent Owner { get; set; }
        public Agent Pilot { get; set; }
        public Dictionary<Int64, Store> Stores { get; set; }
        public SolarSystem SolarSystem { get; set; }
        public Planet DockedPlanet { get; set; }

        public ICollection<Agent> Agents { get; set; }

        public Int64 DestinationScId {get; set;}
    }
}
