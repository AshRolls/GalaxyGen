using GalaxyGenEngine.Engine;
using GalaxyGenEngine.Model;
using GalaxyGenCore;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.ViewModel
{
    public class ResourceTypeViewModel : IResourceTypeViewModel
    {
        private ResourceType model_Var;
        public ResourceType Model
        {
            get { return model_Var; }
            set
            {                
                model_Var = value;
                OnPropertyChanged("Model");
            }
        }

        public ResourceTypeEnum Type
        {
            get
            {
                if (model_Var != null)
                    return model_Var.Type;
                else
                    return 0;
            }
        }

        public String Name
        {
            get {
                if (model_Var != null)
                    return model_Var.Name;
                else
                    return null;
            }
        }

        public Double VolumePerUnit
        {
            get
            {
                if (model_Var != null)
                    return model_Var.VolumePerUnit;
                else
                    return Double.NaN;
            }            
        }

        public Int64 DefaultTicksToProduce
        {
            get
            {
                if (model_Var != null)
                    return model_Var.DefaultTicksToProduce;
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
