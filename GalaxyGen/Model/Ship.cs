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
    public class Ship : ModelNotifyBase
    {
        [Key]
        public Int64 ShipId { get; set; }
      
        [StringLength(60)]
        public String Name { get; set; }

        private Boolean docked_Var;
        public Boolean Docked
        {
            get
            {
                return docked_Var;
            }
            set
            {
                docked_Var = value;
                OnPropertyChanged("Docked");
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

        public virtual Agent Owner { get; set; }
        
        [Required]
        public virtual Store Store { get; set; }

        public virtual SolarSystem SolarSystem { get; set; }
        public virtual Planet DockedPlanet { get; set; }

        [NotMapped]
        public IActorRef Actor { get; set; }
    }
}
