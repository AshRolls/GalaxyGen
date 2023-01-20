using Akka.Actor;
using GalaxyGenEngine.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GalaxyGenEngine.ViewModel
{
    public class GalaxyViewModel : IGalaxyViewModel
    {
        ISolarSystemViewModelFactory solarSystemViewModelFactory;
        private Timer _refreshTimer;

        public GalaxyViewModel(ISolarSystemViewModelFactory initSolarSystemViewModelFactory)
        {
            solarSystemViewModelFactory = initSolarSystemViewModelFactory;
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
            OnPropertyChanged("Name");
            OnPropertyChanged("CurrentTick");
            OnPropertyChanged("TicksPerSecond");            
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

        public UInt64 CurrentTick
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

        public UInt64 TicksPerSecond
        {
            get
            {
                if (model_Var != null)
                    return model_Var.TicksPerSecond;
                else
                    return 0;
            }
            set
            {
                if (model_Var != null)
                {
                    model_Var.TicksPerSecond = value;
                    OnPropertyChanged("TicksPerSecond");
                }
            }
        }

        public ICollection<ScSolarSystem> ScSolarSystems
        {
            get
            {
                return StarChart.SolarSystems.Values;
            }
        }

        private ObservableCollection<IResourceTypeViewModel> resourceTypes_Var = new ObservableCollection<IResourceTypeViewModel>();
        public ObservableCollection<IResourceTypeViewModel> ResourceTypes
        {
            get
            {
                return resourceTypes_Var;
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
