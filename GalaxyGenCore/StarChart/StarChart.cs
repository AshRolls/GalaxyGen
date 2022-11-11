using GalaxyGenCore.Framework;
using System;
using System.Collections.Generic;

namespace GalaxyGenCore.StarChart
{
    public static class StarChart
    {
        private static Dictionary<Int64, ScSolarSystem> _scSolarSystems = new Dictionary<Int64, ScSolarSystem>();
        private static Dictionary<Int64, ScPlanet> _scPlanets = new Dictionary<Int64, ScPlanet>();

        public static void InitialiseStarChart()
        {
            // read from json and initialise _starChart
            ScGalaxy gal = GeneralJsonSerializer.Deserialize<ScGalaxy>(Globals.GALAXY_CORE_DB, null);

            Int64 i = 1;
            Int64 j = 10000000;
            foreach (ScSolarSystem ss in gal.SolarSystems)
            {
                _scSolarSystems.Add(i, ss);
                i++;
                foreach (ScPlanet p in ss.Planets)
                {
                    _scPlanets.Add(j, p);
                    j++;
                }
            }            
        }

        public static Dictionary<Int64,ScSolarSystem> SolarSystems
        {
            get
            {
                return _scSolarSystems;
            }
        }

        public static ScSolarSystem GetSolarSystem(Int64 id)
        {
            return GetEntry<ScSolarSystem>(_scSolarSystems, id);
        }

        public static ScPlanet GetPlanet(Int64 id)
        {
            return GetEntry<ScPlanet>(_scPlanets, id);
        }

        private static T GetEntry<T>(Dictionary<Int64, T> table, Int64 id)
        {
            T local;
            if ((table != null) && table.TryGetValue(id, out local))
            {
                return local;
            }
            return default(T);
        }

        public static Int64 GetIdForObject(ScSolarSystem obj)
        {
            return GetID<ScSolarSystem>(_scSolarSystems, obj);
        }

        public static Int64 GetIdForObject(ScPlanet obj)
        {
            return GetID<ScPlanet>(_scPlanets, obj);
        }

        private static Int64 GetID<T>(Dictionary<Int64, T> table, T obj) where T : class
        {
            foreach (KeyValuePair<Int64, T> pair in table)
            {
                if (pair.Value == obj)
                {
                    return pair.Key;
                }
            }
            return -1;
        }               
                
    }


}
