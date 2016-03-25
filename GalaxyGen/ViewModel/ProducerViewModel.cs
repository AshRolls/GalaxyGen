using Akka.Actor;
using GalaxyGen.Engine;
using GalaxyGen.Framework;
using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.ViewModel
{
    public class ProducerViewModel : IProducerViewModel
    {
        public IActorRef Actor
        {
            get
            {
                if (model_Var != null)
                    return model_Var.Actor;
                else
                    return null;
            }
        }

        private Producer model_Var;
        public Producer Model
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
            Name = model_Var.Name;
            Owner = model_Var.Owner;
            Planet = model_Var.Planet;           
        }

        public String Name
        {
            get {
                if (model_Var != null)
                    return model_Var.Name;
                else
                    return null;
            }
            set {
                if (model_Var != null)
                {
                    model_Var.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public Agent Owner
        {
            get
            {
                if (model_Var != null)
                    return model_Var.Owner;
                else
                    return null;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.Owner = value;
                    OnPropertyChanged("Owner");
                }
            }
        }

        public Planet Planet
        {
            get
            {
                if (model_Var != null)
                    return model_Var.Planet;
                else
                    return null;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.Planet = value;
                    OnPropertyChanged("Planet");
                }
            }
        }

        public BluePrintEnum BluePrintType
        {
            get
            {
                if (model_Var != null)
                    return model_Var.BluePrintType;
                else
                    return BluePrintEnum.NotSet;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.BluePrintType = value;
                    OnPropertyChanged("BluePrintType");
                }
            }
        }

        public Int64 TickForNextProduction
        {
            get
            {
                if (model_Var != null)
                    return model_Var.TickForNextProduction;
                else
                    return 0;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.TickForNextProduction = value;
                    OnPropertyChanged("TickForNextProduction");
                }
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
