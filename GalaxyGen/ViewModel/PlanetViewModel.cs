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
    public class PlanetViewModel : IPlanetViewModel
    {
        private IProducerViewModelFactory _producerVmFactory;

        public PlanetViewModel(ISocietyViewModelFactory initSocietyViewModelFactory, IProducerViewModelFactory initProducerVmFactory)
        {            
            _producerVmFactory = initProducerVmFactory;
            ISocietyViewModelFactory societyViewModelFactory = initSocietyViewModelFactory;
            this.Society = societyViewModelFactory.CreateSocietyViewModel();
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

        private Planet model_Var;
        public Planet Model
        {
            get { return model_Var; }
            set
            {
                if (model_Var != null)
                    model_Var.PropertyChanged -= Model_Var_PropertyChanged;

                model_Var = value;
                updateFromModel();

                if (model_Var != null)
                    model_Var.PropertyChanged += Model_Var_PropertyChanged;

                OnPropertyChanged("Model");
            }
        }

        private void updateFromModel()
        {
            Name = model_Var.Name;
            Population = model_Var.Population;           
            societyVm_Var.Model = model_Var.Society;

            foreach (Producer prod in model_Var.Producers)
            {
                IProducerViewModel prodVm = _producerVmFactory.CreateProducerViewModel();
                prodVm.Model = prod;
                producers_Var.Add(prodVm);
            }
        }

        private void Model_Var_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PositionX")
            {
                OnPropertyChanged("PositionX");
                updatePosX300();
            }
            else if (e.PropertyName == "PositionY")
            {
                OnPropertyChanged("PositionY");
                updatePosY300();
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

        private void updatePosX300()
        {
            Double posX = PositionX / 3000000;
            PosX300 = (int)posX + 150;
        }

        private int posX300_Var;
        public int PosX300
        {
            get
            {
                return posX300_Var;
            }
            private set
            {
                posX300_Var = value;
                OnPropertyChanged("PosX300");
            }
        }

        private void updatePosY300()
        {
            Double posY = PositionY / 3000000;
            PosY300 = (int)posY + 150;
        }

        private int posY300_Var;
        public int PosY300
        {
            get
            {
                return posY300_Var;
            }
            private set
            {
                posY300_Var = value;
                OnPropertyChanged("PosY300");
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
