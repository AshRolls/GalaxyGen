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

        public AgentViewModel(IProducerViewModelFactory initProducerViewModelFactory)
        {
            _producerVmFactory = initProducerViewModelFactory;
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

        public SolarSystem ss
        {
            get
            {
                if (model_Var != null)
                    return model_Var.SolarSystem;
                else
                    return null;
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
