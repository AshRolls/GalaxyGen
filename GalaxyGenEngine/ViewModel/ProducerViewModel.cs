using GalaxyGenEngine.Model;
using GalaxyGenCore.BluePrints;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace GalaxyGenEngine.ViewModel
{
    public class ProducerViewModel : IProducerViewModel
    {
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
            OwnerId = model_Var.OwnerId;                       
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

        public ulong OwnerId
        {
            get
            {
                if (model_Var != null)
                    return model_Var.OwnerId;
                else
                    return 0;
            }    
            set {
                if (model_Var != null)
                {
                    model_Var.OwnerId = value;
                    OnPropertyChanged("OwnerId");
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

        public UInt64 TickForNextProduction
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

        public bool Producing
        {
            get
            {
                if (model_Var != null)
                    return model_Var.Producing;
                else
                    return false;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.Producing = value;
                    OnPropertyChanged("Producing");
                }
            }
        }

        public bool AutoResumeProduction
        {
            get
            {
                if (model_Var != null)
                    return model_Var.AutoResumeProduction;
                else
                    return false;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.AutoResumeProduction = value;
                    OnPropertyChanged("AutoResumeProduction");
                }
            }
        }

        public int ProduceNThenStop
        {
            get
            {
                if (model_Var != null)
                    return model_Var.ProduceNThenStop;
                else
                    return 0;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.ProduceNThenStop = value;
                    OnPropertyChanged("ProduceNThenStop");
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
