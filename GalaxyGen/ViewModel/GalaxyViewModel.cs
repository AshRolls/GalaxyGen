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
    public class GalaxyViewModel : IGalaxyViewModel
    {
        ISolarSystemViewModelFactory solarSystemViewModelFactory;       

        public GalaxyViewModel(ISolarSystemViewModelFactory initSolarSystemViewModelFactory)
        {
            solarSystemViewModelFactory = initSolarSystemViewModelFactory;            
        }

        private Galaxy model_Var;
        public Galaxy Model
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
            foreach(SolarSystem ss in model_Var.SolarSystems)
            {
                ISolarSystemViewModel ssVm = solarSystemViewModelFactory.CreateSolarSystemViewModel();
                ssVm.Model = ss;
                solarSystems_Var.Add(ssVm);
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

        public Int64 CurrentTick
        {
            get
            {
                if (model_Var != null)
                    return model_Var.CurrentTick;
                else
                    return 0;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.CurrentTick = value;
                    OnPropertyChanged("CurrentTick");
                }
            }
        }

        private ObservableCollection<ISolarSystemViewModel> solarSystems_Var = new ObservableCollection<ISolarSystemViewModel>();
        public ObservableCollection<ISolarSystemViewModel> SolarSystems
        {
            get
            {
                return solarSystems_Var;
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
