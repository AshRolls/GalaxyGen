﻿using GalaxyGen.Engine;
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

namespace GalaxyGen.ViewModel
{
    public class MainViewModel : IMainViewModel
    {
        IGalaxyCreator _galaxyCreator;
        IPlanetViewModelFactory _planetViewModelFactory;
        ResourceTypeInitialiser _resourceTypeInitialiser;
        GalaxyContext _db;
        
        public MainViewModel(IGalaxyCreator initGalaxyCreator, IPlanetViewModelFactory initPlanetViewModelFactory)
        {
            _galaxyCreator = initGalaxyCreator;
            _planetViewModelFactory = initPlanetViewModelFactory;

            _resourceTypeInitialiser = new ResourceTypeInitialiser();
            _db = new GalaxyContext();

            loadOrCreateGalaxy();           
        }

        private void loadOrCreateGalaxy()
        {
            Planet planet;


            planet = _db.Planets.FirstOrDefault();
            if (planet == null)
            {
                planet = _galaxyCreator.GetPlanet();
                _db.Planets.Add(planet);
                _db.Societies.Add(planet.Soc);

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
            

            IPlanetViewModel planetViewModel;
            if (planet != null)
            {
                planetViewModel = _planetViewModelFactory.CreatePlanetViewModel();
                planetViewModel.Model = planet;
                planets_Var.Add(planetViewModel);
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
