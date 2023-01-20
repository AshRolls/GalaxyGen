using GalaxyGenCore.Resources;
using GalaxyGenEngine.Engine.Controllers;
using GalaxyGenEngine.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace GalaxyGenEngine.ViewModel
{
    public class MarketViewModel : IMarketViewModel
    {       
        public MarketViewModel()
        {
            
        }

        private Market model_Var;
        public Market Model
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
