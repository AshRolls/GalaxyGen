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
        Cruising
    }

    public class Ship : ModelNotifyBase, IAgentLocation, IStoreLocation
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
      
        [StringLength(60)]
        public String Name { get; set; }

        private ShipStateEnum shipState_Var;
        public ShipStateEnum ShipState
        {
            get
            {
                return shipState_Var;
            }
            set
            {
                shipState_Var = value;
                OnPropertyChanged("ShipState");
            }
        }

        private Double positionX_Var;
        public Double PositionX
        {
            get
            {
                return positionX_Var;
            }
            set
            {
                positionX_Var = value;
                OnPropertyChanged("PositionX");
            }
        }

        private Double positionY_Var;
        public Double PositionY
        {
            get
            {
                return positionY_Var;
            }
            set
            {
                positionY_Var = value;
                OnPropertyChanged("PositionY");
            }
        }

        public Agent Owner { get; set; }
        public Dictionary<Int64, Store> Stores { get; set; }
        public SolarSystem SolarSystem { get; set; }
        public Planet DockedPlanet { get; set; }
        public ICollection<Agent> Agents { get; set; }

        [JsonIgnore]
        public IActorRef Actor { get; set; }
    }
}
