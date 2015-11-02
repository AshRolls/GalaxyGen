using GalaxyGen.Engine;
using GalaxyGen.Model;
using System;
using System.Collections.ObjectModel;
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
        
        public MainViewModel(IGalaxyCreator initGalaxyCreator, IPlanetViewModelFactory initPlanetViewModelFactory)
        {
            _galaxyCreator = initGalaxyCreator;
            _planetViewModelFactory = initPlanetViewModelFactory;

            _resourceTypeInitialiser = new ResourceTypeInitialiser();

            loadOrCreateGalaxy();           
        }

        private void loadOrCreateGalaxy()
        {
            IPlanet planet;

            using (GalaxyContext db = new GalaxyContext())
            {
                planet = db.Planets.FirstOrDefault();
                if (planet == null)
                {
                    planet = _galaxyCreator.GetPlanet();
                    db.Planets.Add((Planet)planet);

                    try
                    {
                        db.SaveChanges();
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
    }
}
