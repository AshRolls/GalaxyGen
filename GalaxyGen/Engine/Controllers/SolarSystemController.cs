using GalaxyGen.Framework;
using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class SolarSystemController
    {
        private SolarSystem _model;

        public SolarSystemController(SolarSystem ss)
        {
            _model = ss;
        }

        public void Tick(MessageTick tick)
        {
            movePlanets(tick);
        }

        private void movePlanets(MessageTick tick)
        {
            foreach (Planet p in _model.Planets)
            {
                PointD pt = OrbitalUtils.CalcPositionFromTick(tick.Tick, p.OrbitDays, p.OrbitKm);
                p.PositionX = pt.X;
                p.PositionY = pt.Y;
            }
        }
    }
}
