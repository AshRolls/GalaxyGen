using GalaSoft.MvvmLight.CommandWpf;
using GalaxyGen.Engine;
using GalaxyGen.Framework;
using GalaxyGen.Model;
using GalaxyGenCore;
using GalaxyGenCore.StarChart;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalaxyGen.ViewModel
{
    public class MainGalaxyViewModel : IMainGalaxyViewModel
    {        
        IGalaxyPopulator _galaxyCreator;
        ITickEngine _tickEngine;

        IGalaxyViewModelFactory _galaxyViewModelFactory;
        ISolarSystemViewModelFactory _solarSystemViewModelFactory;
        IPlanetViewModelFactory _planetViewModelFactory;        
        
        // should improve this system don't need them hanging around
        ResourceTypeInitialiser _resourceTypeInitialiser;  
        
        public MainGalaxyViewModel(IGalaxyPopulator initGalaxyCreator, 
                                    IGalaxyViewModelFactory initGalaxyViewModelFactory, 
                                    ISolarSystemViewModelFactory initSolarSystemViewModelFactory, 
                                    IPlanetViewModelFactory initPlanetViewModelFactory, 
                                    ITextOutputViewModel initTextOutputViewModel,
                                    ITickEngine initTickEngine)
        {
            _galaxyCreator = initGalaxyCreator;
            _tickEngine = initTickEngine;

            _galaxyViewModelFactory = initGalaxyViewModelFactory;
            _solarSystemViewModelFactory = initSolarSystemViewModelFactory;
            _planetViewModelFactory = initPlanetViewModelFactory;

            TextOutput = initTextOutputViewModel;

            StarChart.InitialiseStarChart();
            _resourceTypeInitialiser = new ResourceTypeInitialiser(); // TODO modify resources to use same sys as bp
            BluePrints.initialiseBluePrints();

            loadOrCreateGalaxy();
            initialiseEngine();

            saveGalaxy();      
        }

        private void loadOrCreateGalaxy()
        {
            Galaxy gal = GalaxyLoader.Load();
            if (gal == null)
            {
                IdUtils.currentId = 100;
                gal = _galaxyCreator.GetFullGalaxy();
                GalaxyLoader.Save(gal);
            }
            else
            {
                IdUtils.currentId = gal.MaxId;
            }

            galaxyViewModel_Var = _galaxyViewModelFactory.CreateGalaxyViewModel();
            galaxyViewModel_Var.Model = gal;                                    
        }

        private void saveGalaxy()
        {
            galaxyViewModel_Var.Model.MaxId = IdUtils.currentId;
            GalaxyLoader.Save(galaxyViewModel_Var.Model);
        }

        private void initialiseEngine()
        {
            _tickEngine.SetupTickEngine(galaxyViewModel_Var, textOutput_Var);
        }        

        private IGalaxyViewModel galaxyViewModel_Var;
        public IGalaxyViewModel Galaxy
        {
            get
            {
                return galaxyViewModel_Var;
            }
            set
            {
                galaxyViewModel_Var = value;
            }
        }

        private ISolarSystemViewModel selectedSolarSystem_Var;
        public ISolarSystemViewModel SelectedSolarSystem
        {
            get
            {
                return selectedSolarSystem_Var;
            }
            set
            {
                selectedSolarSystem_Var = value;
                OnPropertyChanged("SelectedSolarSystem");
            }
        }

        private IPlanetViewModel selectedPlanet_Var;
        public IPlanetViewModel SelectedPlanet
        {
            get
            {
                return selectedPlanet_Var;
            }
            set
            {
                selectedPlanet_Var = value;
                OnPropertyChanged("SelectedPlanet");                
            }
        }


        private IAgentViewModel selectedAgent_Var;
        public IAgentViewModel SelectedAgent
        {
            get
            {
                return selectedAgent_Var;
            }
            set
            {
                selectedAgent_Var = value;
                OnPropertyChanged("SelectedAgent");
            }
        }

        private ITextOutputViewModel textOutput_Var;
        public ITextOutputViewModel TextOutput
        {
            get
            {
                return textOutput_Var;
            }
            set
            {
                textOutput_Var = value;
                OnPropertyChanged("TextOutput");
            }
        }

        private RelayCommand windowClosingCommand;
        public ICommand WindowClosing
        {
            get
            {
                if (windowClosingCommand == null) windowClosingCommand = new RelayCommand(() => saveGalaxy());
                return windowClosingCommand;
            }
        }


        private RelayCommand runMaxEngineCommand;
        public ICommand RunMaxEngineCommand
        {
            get
            {
                if (runMaxEngineCommand == null) runMaxEngineCommand = new RelayCommand(() => runEngine(true), () => canRunEngine());
                return runMaxEngineCommand;
            }
        }

        private RelayCommand runEngineCommand;
        public ICommand RunEngineCommand
        {
            get
            {
                if (runEngineCommand == null) runEngineCommand = new RelayCommand(() => runEngine(false), () => canRunEngine());
                return runEngineCommand; 
            }
        }

        private void runEngine(bool maxRate)
        {            
            _tickEngine.Run(maxRate);
        }

        private bool canRunEngine()
        {
            return true;
        }

        private RelayCommand stopEngineCommand;
        public ICommand StopEngineCommand
        {
            get
            {
                if (stopEngineCommand == null) stopEngineCommand = new RelayCommand(() => stopEngine(), () => canStopEngine());
                return stopEngineCommand;
            }
        }

        private void stopEngine()
        {
            _tickEngine.Stop();
        }

        private bool canStopEngine()
        {
            return true;
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
