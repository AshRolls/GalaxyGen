using Akka.Actor;
using GCEngine.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.ViewModel
{
    public class SolarSystemViewModel : ISolarSystemViewModel
    {
        IPlanetViewModelFactory planetViewModelFactory;
        IAgentViewModelFactory agentViewModelFactory;
        IShipViewModelFactory shipViewModelFactory;

        public SolarSystemViewModel(IPlanetViewModelFactory initPlanetViewModelFactory, IAgentViewModelFactory initAgentViewModelFactory, IShipViewModelFactory initShipViewModelFactory)
        {
            planetViewModelFactory = initPlanetViewModelFactory;
            agentViewModelFactory = initAgentViewModelFactory;
            shipViewModelFactory = initShipViewModelFactory;
        }

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

        private ObservableCollection<IPlanetViewModel> planets_Var;
        public ObservableCollection<IPlanetViewModel> Planets
        {
            get
            {
                if (planets_Var == null) initialisePlanets();
                return planets_Var;
            }            
        }

        private void initialisePlanets()
        {
            planets_Var = new ObservableCollection<IPlanetViewModel>();
            if (model_Var != null)
            {
                foreach (Planet p in model_Var.Planets.Values)
                {
                    IPlanetViewModel pVm = planetViewModelFactory.CreatePlanetViewModel();
                    pVm.Model = p;
                    planets_Var.Add(pVm);
                }
            }
        }

        private ObservableCollection<IAgentViewModel> agents_Var;
        public ObservableCollection<IAgentViewModel> Agents
        {
            get
            {
                if (agents_Var == null) initialiseAgents();
                return agents_Var;
            }
        }

        private void initialiseAgents()
        {
            agents_Var = new ObservableCollection<IAgentViewModel>();
            if (model_Var != null)
            {
                foreach (Agent ag in model_Var.Agents)
                {
                    IAgentViewModel agVm = agentViewModelFactory.CreateAgentViewModel();
                    agVm.Model = ag;
                    agents_Var.Add(agVm);
                }
            }
        }

        private ObservableCollection<IShipViewModel> ships_Var;
        public ObservableCollection<IShipViewModel> Ships
        {
            get
            {
                if (ships_Var == null) initialiseShips();
                return ships_Var;
            }
        }

        private void initialiseShips()
        {
            ships_Var = new ObservableCollection<IShipViewModel>();
            if (model_Var != null)
            {
                foreach (Ship s in model_Var.Ships.Values)
                {
                    IShipViewModel sVm = shipViewModelFactory.CreateShipViewModel();
                    sVm.Model = s;
                    ships_Var.Add(sVm);
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
