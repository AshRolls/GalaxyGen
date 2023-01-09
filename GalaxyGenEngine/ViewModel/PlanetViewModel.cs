using GCEngine.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;

namespace GCEngine.ViewModel
{
    public class PlanetViewModel : IPlanetViewModel
    {
        private IProducerViewModelFactory _producerVmFactory;
        private Timer _refreshTimer;

        public PlanetViewModel(ISocietyViewModelFactory initSocietyVMFactory, IProducerViewModelFactory initProducerVmFactory, IMarketViewModelFactory initMarketVmFactory)
        {            
            _producerVmFactory = initProducerVmFactory;
            ISocietyViewModelFactory societyViewModelFactory = initSocietyVMFactory;
            this.Society = societyViewModelFactory.CreateSocietyViewModel();
            IMarketViewModelFactory marketViewModelFactory = initMarketVmFactory;
            this.Market = marketViewModelFactory.CreateMarketViewModel();
            setupAndStartTimer();
        }

        private void setupAndStartTimer()
        {
            _refreshTimer = new Timer(50);
            _refreshTimer.Elapsed += _refreshTimer_Elapsed;
            _refreshTimer.Start();
        }

        private void _refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            refreshAllProperties();
        }

        private void refreshAllProperties()
        {
            OnPropertyChanged("PositionX");
            OnPropertyChanged("PositionY");
            updatePosX800();
            updatePosY800();
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

        public Double PositionX
        {
            get
            {
                if (model_Var != null)
                    return model_Var.PositionX;
                else
                    return 0;
            }
        }

        public Double PositionY
        {
            get
            {
                if (model_Var != null)
                    return model_Var.PositionY;
                else
                    return 0;
            }
        }

        private void updatePosX800()
        {
            Double posX = PositionX / 3000000;
            PosX800 = (int)posX + 400;
        }

        private int posX800_Var;
        public int PosX800
        {
            get
            {
                return posX800_Var;
            }
            private set
            {
                posX800_Var = value;
                OnPropertyChanged("PosX800");
            }
        }

        private void updatePosY800()
        {
            Double posY = PositionY / 3000000;
            PosY800 = (int)posY + 400;
        }

        private int posY800_Var;
        public int PosY800
        {
            get
            {
                return posY800_Var;
            }
            private set
            {
                posY800_Var = value;
                OnPropertyChanged("PosY800");
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
