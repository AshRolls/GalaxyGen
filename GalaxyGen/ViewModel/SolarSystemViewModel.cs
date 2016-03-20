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
    public class SolarSystemViewModel : ISolarSystemViewModel
    {
        IPlanetViewModelFactory planetViewModelFactory;
        IAgentViewModelFactory agentViewModelFactory;

        public SolarSystemViewModel(IPlanetViewModelFactory initPlanetViewModelFactory, IAgentViewModelFactory initAgentViewModelFactory)
        {
            planetViewModelFactory = initPlanetViewModelFactory;
            agentViewModelFactory = initAgentViewModelFactory;
        }

        private SolarSystem model_Var;
        public SolarSystem Model
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
            foreach(Planet p in model_Var.Planets)
            {
                IPlanetViewModel pVm = planetViewModelFactory.CreatePlanetViewModel();
                pVm.Model = p;
                planets_Var.Add(pVm);
            }
            foreach (Agent ag in model_Var.Agents)
            {
                IAgentViewModel agVm = agentViewModelFactory.CreateAgentViewModel();
                agVm.Model = ag;
                agents_Var.Add(agVm);
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

        private ObservableCollection<IPlanetViewModel> planets_Var = new ObservableCollection<IPlanetViewModel>();
        public ObservableCollection<IPlanetViewModel> Planets
        {
            get
            {
                return planets_Var;
            }            
        }

        private ObservableCollection<IAgentViewModel> agents_Var = new ObservableCollection<IAgentViewModel>();
        public ObservableCollection<IAgentViewModel> Agents
        {
            get
            {
                return agents_Var;
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
