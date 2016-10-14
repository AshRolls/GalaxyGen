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
                ss.Planets.Add(getPlanet("Earth", 150000000d, 365));
                ss.Planets.Add(getPlanet("Mars", 227000000d, 687));

                gal.SolarSystems.Add(ss);                
            }
            else
            {
                for (int i = 0; i < numberOfSystems; i++)
                {
                    ScSolarSystem ss = new ScSolarSystem();
                    ss.Planets = new HashSet<ScPlanet>();
                    ss.Name = "SolarSystem" + i.ToString();
                    ss.Planets.Add(getPlanet("P1", 100000000d, 100));
                    ss.Planets.Add(getPlanet("P2", 150000000d, 200));
                    ss.Planets.Add(getPlanet("P3", 200000000d, 300));
                    ss.Planets.Add(getPlanet("P4", 250000000d, 400));
                    gal.SolarSystems.Add(ss);
                }
            }
            return gal;
        }

        private ScPlanet getPlanet(string name, Double orbitalKm, Double orbitalDays)
        {
            ScPlanet plan = new ScPlanet();
            plan.OrbitKm = orbitalKm;
            plan.OrbitDays = orbitalDays;
            plan.Name = name;
            return plan;
        }
    }
}
