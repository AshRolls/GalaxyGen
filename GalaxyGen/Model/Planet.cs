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
    public class Planet : ModelNotifyBase, IAgentLocation, IStoreLocation
    {
        public Planet()
        {
            PlanetId = IdUtils.GetId();
            GalType = TypeEnum.Planet;
            Producers = new HashSet<Producer>();
            Stores = new Dictionary<Int64,Store>();
            DockedShips = new List<Ship>();
            Agents = new List<Agent>();
        }

        public Int64 PlanetId { get; set; }
        public TypeEnum GalType { get; set; }

        public Double OrbitKm { get; set; }
        public Double OrbitDays { get; set; }

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

        [StringLength(60)]
        public String Name { get; set; }
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
