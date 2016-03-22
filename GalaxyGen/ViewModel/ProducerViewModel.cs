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
            List<int> resProd = GalaxyJsonSerializer.Deserialize(model_Var.ResourcesProducedJsonSerialized);
            foreach (int i in resProd)
            {
                resourcesProduced_Var.Add(ResourceTypes.Types[i]);
            }
            List<int> resConsume = GalaxyJsonSerializer.Deserialize(model_Var.ResourcesConsumedJsonSerialized);
            foreach (int i in resConsume)
            {
                resourcesConsumed_Var.Add(ResourceTypes.Types[i]);
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

        private ObservableCollection<ResourceType> resourcesProduced_Var = new ObservableCollection<ResourceType>();
        public ObservableCollection<ResourceType> ResourcesProduced
        {
            get
            {
                return resourcesProduced_Var;
            }
            set
            {
                resourcesProduced_Var = value;
                if (model_Var != null)
                {
                    model_Var.ResourcesConsumedJsonSerialized = serializeResourceTypes(resourcesProduced_Var);
                }
                OnPropertyChanged("ResourcesProduced");
            }
        }

        private ObservableCollection<ResourceType> resourcesConsumed_Var = new ObservableCollection<ResourceType>();
        public ObservableCollection<ResourceType> ResourcesConsumed
        {
            get
            {
                return resourcesConsumed_Var;
            }
            set
            {
                resourcesConsumed_Var = value;
                if (model_Var != null)
                {
                    model_Var.ResourcesConsumedJsonSerialized = serializeResourceTypes(resourcesConsumed_Var);
                }
                OnPropertyChanged("ResourcesProduced");
            }
        }

        private String serializeResourceTypes(ObservableCollection<ResourceType> types)
        {
            List<int> typeInts = new List<int>();
            foreach(ResourceType res in types)
            {
                typeInts.Add((int)res.Type);
            }
            return GalaxyJsonSerializer.Serialize(typeInts);
        }

        //List<ResourceType> ResourcesConsumed { get; set; }

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
