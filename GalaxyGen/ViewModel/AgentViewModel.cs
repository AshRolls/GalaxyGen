using Akka.Actor;
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
    public class AgentViewModel : IAgentViewModel
    {
        private IProducerViewModelFactory _producerVmFactory;
        private ISolarSystemViewModelFactory _ssVmFactory;

        public AgentViewModel(IProducerViewModelFactory initProducerViewModelFactory, ISolarSystemViewModelFactory initSsVmFactory)
        {
            _producerVmFactory = initProducerViewModelFactory;
            _ssVmFactory = initSsVmFactory;                      
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

        private Agent model_Var;
        public Agent Model
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

            foreach (Producer prod in model_Var.Producers)
            {
                IProducerViewModel prodVm = _producerVmFactory.CreateProducerViewModel();
                prodVm.Model = prod;
                producers_Var.Add(prodVm);
            }

            //SolarSystem = _ssVmFactory.CreateSolarSystemViewModel();
            //SolarSystem.Model = model_Var.SolarSystem;
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

        private ISolarSystemViewModel solarSystem_Var;
        public ISolarSystemViewModel SolarSystem
        {
            get
            {
                return solarSystem_Var;
            }
            set
            {
                solarSystem_Var = value;
                OnPropertyChanged("SolarSystem");
            }           
        }

        private ObservableCollection<IProducerViewModel> producers_Var = new ObservableCollection<IProducerViewModel>();
        public ObservableCollection<IProducerViewModel> Producers
        {
            get
            {
                return producers_Var;
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
