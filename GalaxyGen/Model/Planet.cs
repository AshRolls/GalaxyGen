using Akka.Actor;
using GalaxyGen.Framework;
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
    public class Planet : ModelNotifyBase
    {
        public Planet()
        {
            Producers = new HashSet<Producer>();
            Stores = new HashSet<Store>();
            DockedShips = new List<Ship>();
        }

        [Key]
        public Int64 PlanetId { get; set; }

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

        public Int64 SocietyId { get; set; }
        [ForeignKey("SocietyId")]
        public virtual Society Society { get; set; }

        public virtual ICollection<Producer> Producers { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<Ship> DockedShips { get; set; }

        public SolarSystem SolarSystem { get; set; }

        [NotMapped]
        public IActorRef Actor { get; set; }

        //[ForeignKey("MarketId")]
        //public IMarket Market { get; set; }

    }
}
