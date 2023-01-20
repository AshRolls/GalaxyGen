using Akka.Actor;
using GalaxyGenEngine.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GalaxyGenEngine.ViewModel
{
    public class ShipViewModel : IShipViewModel
    {        
        public ShipViewModel()
        {            
        }

        private Ship model_Var;
        public Ship Model
        {
            get { return model_Var; }
            set
            {

                model_Var = value;
                updateFromModel();

                OnPropertyChanged("Model");
            }
        }

        private void updateFromModel()
        {
            name_Var = model_Var.Name;     
        }

        private string name_Var;
        public String Name
        {
            get {
                return name_Var;
            }
        }

        public Double PositionX
        {
            get
            {
                if (model_Var != null)
                    return model_Var.PositionX;
                else
                    return 0;
            }
        }

        public Double PositionY
        {
            get
            {
                if (model_Var != null)
                    return model_Var.PositionY;
                else
                    return 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
