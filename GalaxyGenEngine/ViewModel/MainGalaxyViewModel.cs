using GalaxyGenEngine.Engine;
using GalaxyGenEngine.Framework;
using GalaxyGenEngine.Model;
using GalaxyGenCore.BluePrints;
using GalaxyGenCore.Resources;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace GalaxyGenEngine.ViewModel
{
    public class MainGalaxyViewModel : IMainGalaxyViewModel
    {        
        IGalaxyPopulator _galaxyCreator;
        ITickEngine _tickEngine;

        IGalaxyViewModelFactory _galaxyViewModelFactory;
        ISolarSystemViewModelFactory _solarSystemViewModelFactory;
        IPlanetViewModelFactory _planetViewModelFactory;        
                       
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
            ResourceTypes.InitialiseResourceTypes();
            BluePrints.InitialiseBluePrints();

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
            stopEngine();
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
        public ISolarSystemViewModel SelectedSolarSystemVm
        {
            get
            {
                return selectedSolarSystem_Var;
            }
            private set
            {
                selectedSolarSystem_Var = value;
                OnPropertyChanged("SelectedSolarSystemVm");
            }
        }

        private ScSolarSystem selectedScSolarSystem_Var;
        public ScSolarSystem SelectedScSolarSystem
        {
            get
            {
                return selectedScSolarSystem_Var;
            }
            set
            {
                selectedScSolarSystem_Var = value;
                createSsVmFromSelectedScSS();

                OnPropertyChanged("SelectedScSolarSystem");
            }
        }

        private void createSsVmFromSelectedScSS()
        {
            UInt64 scId = StarChart.GetIdForObject(selectedScSolarSystem_Var);
            SolarSystem ss = Galaxy.Model.SolarSystems.Where(x => x.StarChartId == scId).FirstOrDefault();
            if (ss != null)
            {
                ISolarSystemViewModel ssVm = _solarSystemViewModelFactory.CreateSolarSystemViewModel();
                ssVm.Model = ss;
                SelectedSolarSystemVm = ssVm;
            }
        }

        private IPlanetViewModel selectedPlanetVm_Var;
        public IPlanetViewModel SelectedPlanetVm
        {
            get
            {
                return selectedPlanetVm_Var;
            }
            private set
            {
                selectedPlanetVm_Var = value;
                OnPropertyChanged("SelectedPlanetVm");
            }
        }

        private ScPlanet selectedScPlanet_Var;
        public ScPlanet SelectedScPlanet
        {
            get
            {
                return selectedScPlanet_Var;
            }
            set
            {
                selectedScPlanet_Var = value;
                createPVmFromSelectedScP();
                OnPropertyChanged("SelectedScPlanet");                
            }
        }

        private void createPVmFromSelectedScP()
        {
            UInt64 pId = StarChart.GetIdForObject(selectedScPlanet_Var);
            Planet p = SelectedSolarSystemVm.Planets.Select(x => x.Model).Where(x => x.StarChartId == pId).FirstOrDefault();
            if (p != null)
            {
                IPlanetViewModel pVm = _planetViewModelFactory.CreatePlanetViewModel();
                pVm.Model = p;
                SelectedPlanetVm = pVm;
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
                if (runMaxEngineCommand == null)
                {
                    runMaxEngineCommand = new RelayCommand(() => runEngine(true));
                    runMaxEngineCommand.IsEnabled = true;
                }
                return runMaxEngineCommand;
            }
        }

        private RelayCommand runEngineCommand;
        public ICommand RunEngineCommand
        {
            get
            {
                if (runEngineCommand == null)
                {
                    runEngineCommand = new RelayCommand(() => runEngine(false));
                    runEngineCommand.IsEnabled = true;
                }
                return runEngineCommand; 
            }
        }

        private void runEngine(bool maxRate)
        {            
            _tickEngine.Run(maxRate);
        }

        private RelayCommand stopEngineCommand;
        public ICommand StopEngineCommand
        {
            get
            {
                if (stopEngineCommand == null)
                {
                    stopEngineCommand = new RelayCommand(() => stopEngine());
                    stopEngineCommand.IsEnabled = true;
                }
                return stopEngineCommand;
            }
        }

        private void stopEngine()
        {
            _tickEngine.Stop();
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
