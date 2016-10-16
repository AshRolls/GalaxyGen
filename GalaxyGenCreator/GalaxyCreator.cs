using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenCreator
{
    public class GalaxyCreator
    {
        public ScGalaxy GenerateGalaxy(int numberOfSystems)
        {
            ScGalaxy gal = new ScGalaxy();
            gal.SolarSystems = new HashSet<ScSolarSystem>();
            if (numberOfSystems == 1)
            {
                ScSolarSystem ss = new ScSolarSystem();                
                ss.Planets = new HashSet<ScPlanet>();
                ss.Name = "Sol";
                ss.Planets.Add(getPlanet("Mercury", 57.9, 87.96));
                ss.Planets.Add(getPlanet("Venus", 108.2, 224.68));
                ss.Planets.Add(getPlanet("Earth", 149.6, 365.26));
                ss.Planets.Add(getPlanet("Mars", 227.9, 686.98));
                ss.Planets.Add(getPlanet("Jupiter", 778.3, 4332.7));
                ss.Planets.Add(getPlanet("Saturn", 1427, 10759.1));
                ss.Planets.Add(getPlanet("Uranus", 2871, 30707.4));
                ss.Planets.Add(getPlanet("Neptune", 4497.1, 60198.5));
                gal.SolarSystems.Add(ss);                
            }
            else
            {
                for (int i = 0; i < numberOfSystems; i++)
                {
                    ScSolarSystem ss = new ScSolarSystem();
                    ss.Planets = new HashSet<ScPlanet>();
                    ss.Name = "SolarSystem" + i.ToString();
                    ss.Planets.Add(getPlanet("P1", 100, 100));
                    ss.Planets.Add(getPlanet("P2", 150, 200));
                    ss.Planets.Add(getPlanet("P3", 200, 300));
                    ss.Planets.Add(getPlanet("P4", 250, 400));
                    gal.SolarSystems.Add(ss);
                }
            }
            return gal;
        }

        private ScPlanet getPlanet(string name, Double orbitalMKm, Double orbitalDays)
        {
            ScPlanet plan = new ScPlanet();
            plan.OrbitKm = orbitalMKm * 1000000d;
            plan.OrbitDays = orbitalDays;
            plan.Name = name;
            return plan;
        }
    }
}
