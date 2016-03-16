﻿using GalaSoft.MvvmLight.CommandWpf;
using GalaxyGen.Engine;
using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalaxyGen.ViewModel
{
    public class MainGalaxyViewModel : IMainGalaxyViewModel
    {
        GalaxyContext _db;
        IGalaxyCreator _galaxyCreator;
        ITickEngine _tickEngine;
        IPlanetViewModelFactory _planetViewModelFactory;
        ResourceTypeInitialiser _resourceTypeInitialiser;        
        
        public MainGalaxyViewModel(IGalaxyCreator initGalaxyCreator, IPlanetViewModelFactory initPlanetViewModelFactory, ITickEngine initTickEngine)
        {
            _galaxyCreator = initGalaxyCreator;
            _tickEngine = initTickEngine;
            _planetViewModelFactory = initPlanetViewModelFactory;

            _resourceTypeInitialiser = new ResourceTypeInitialiser();
            
            loadOrCreateGalaxy();

            saveGalaxy();      
        }       

        private void loadOrCreateGalaxy()
        {
            _db = new GalaxyContext();

            if (_db.Planets.Count() == 0)
            {
                _db.Planets.Add(_galaxyCreator.GetPlanet("Earth"));
                _db.Planets.Add(_galaxyCreator.GetPlanet("Mars"));                
            }      
            
            foreach(Planet p in _db.Planets)
            {
                IPlanetViewModel planetViewModel = _planetViewModelFactory.CreatePlanetViewModel();
                planetViewModel = _planetViewModelFactory.CreatePlanetViewModel();
                planetViewModel.Model = p;
                planets_Var.Add(planetViewModel);
            }        
        }

        private void saveGalaxy()
        {
            try
            {
                _db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        private ObservableCollection<IPlanetViewModel> planets_Var = new ObservableCollection<IPlanetViewModel>();
        public ObservableCollection<IPlanetViewModel> Planets
        {
            get
            {
                return planets_Var;
            }
            set
            {
                planets_Var = value;
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

        private RelayCommand windowClosingCommand;
        public ICommand WindowClosing
        {
            get
            {
                if (windowClosingCommand == null) windowClosingCommand = new RelayCommand(() => saveGalaxy());
                return windowClosingCommand;
            }
        }

        private RelayCommand runEngineCommand;
        public ICommand RunEngineCommand
        {
            get
            {
                if (runEngineCommand == null) runEngineCommand = new RelayCommand(() => runEngine(), () => canRunEngine());
                return runEngineCommand; 
            }
        }

        private void runEngine()
        {
            _tickEngine.RunTick(1);
        }

        private bool canRunEngine()
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