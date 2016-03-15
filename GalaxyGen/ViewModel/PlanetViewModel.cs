using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.ViewModel
{
    public class PlanetViewModel : IPlanetViewModel
    {

        public PlanetViewModel(ISocietyViewModelFactory initSocietyViewModelFactory)
        {
            ISocietyViewModelFactory _societyViewModelFactory = initSocietyViewModelFactory;
            this.Society = _societyViewModelFactory.CreateSocietyViewModel();
        }

        private Planet model_Var;
        public Planet Model
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
            Population = model_Var.Population;           
            societyVm_Var.Model = model_Var.Society;
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

        public Int64 Population
        {
            get
            {
                if (model_Var != null)
                    return model_Var.Population;
                else
                    return 0;
            }
            set
            {
                if (model_Var != null)
                { 
                    model_Var.Population = value;
                    OnPropertyChanged("Population");
                }
            }
        }

        private ISocietyViewModel societyVm_Var;
        public ISocietyViewModel Society
        {
            get
            {
                return societyVm_Var;
            }
            private set
            {
                societyVm_Var = value;
                OnPropertyChanged("Society");
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
