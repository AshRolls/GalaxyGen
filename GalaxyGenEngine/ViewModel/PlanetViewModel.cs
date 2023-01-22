using GalaxyGenEngine.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GalaxyGenEngine.Framework;

namespace GalaxyGenEngine.ViewModel
{
    public class PlanetViewModel : IPlanetViewModel
    {
        private IProducerViewModelFactory _producerVmFactory;

        public PlanetViewModel(ISocietyViewModelFactory initSocietyVMFactory, IProducerViewModelFactory initProducerVmFactory, IMarketViewModelFactory initMarketVmFactory)
        {            
            _producerVmFactory = initProducerVmFactory;
            ISocietyViewModelFactory societyViewModelFactory = initSocietyVMFactory;
            this.Society = societyViewModelFactory.CreateSocietyViewModel();
            IMarketViewModelFactory marketViewModelFactory = initMarketVmFactory;
            this.Market = marketViewModelFactory.CreateMarketViewModel();
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
            ScPlanet p = StarChart.GetPlanet(model_Var.StarChartId);
            name_Var = p.Name;
            //Population = model_Var.Population;           
            societyVm_Var.Model = model_Var.Society;
            marketVm_Var.Model = model_Var.Market;

            foreach (Producer prod in model_Var.Producers)
            {
                IProducerViewModel prodVm = _producerVmFactory.CreateProducerViewModel();
                prodVm.Model = prod;
                producers_Var.Add(prodVm);
            }
        }

        private string name_Var;
        public String Name
        {
            get {
                return name_Var;
            }
        }

        //public Int64 Population
        //{
        //    get
        //    {
        //        if (model_Var != null)
        //            return model_Var.Population;
        //        else
        //            return 0;
        //    }
        //    set
        //    {
        //        if (model_Var != null)
        //        { 
        //            model_Var.Population = value;
        //            OnPropertyChanged("Population");
        //        }
        //    }
        //}

        public Vector2 Position
        {
            get
            {
                if (model_Var != null)
                    return model_Var.Position;
                else
                    return new Vector2(0,0);
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

        private IMarketViewModel marketVm_Var;
        public IMarketViewModel Market
        {
            get
            {
                return marketVm_Var;
            }
            private set
            {
                marketVm_Var = value;
                OnPropertyChanged("Market");
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
