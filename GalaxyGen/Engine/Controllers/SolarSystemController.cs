using Akka.Actor;
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
        private List<PlanetController> _planetCs;
        private IActorRef _actorTextOutput;      

        public SolarSystemController(SolarSystem ss, IActorRef actorTextOutput)
        {
            _model = ss;
            _planetCs = new List<PlanetController>();
            _actorTextOutput = actorTextOutput;

            // create child controller for each planet in ss
            foreach (Planet p in ss.Planets)
            {
                PlanetController pc = new PlanetController(p, actorTextOutput);
                _planetCs.Add(pc);
            }
        }

        public void Tick(MessageTick tick)
        {
            movePlanetXY(tick);            
            updatePlanets(tick);
        }

        private void updatePlanets(MessageTick tick)
        {
            foreach (PlanetController pc in _planetCs)
            {
                pc.Tick(tick);
            }
        }

        private void movePlanetXY(MessageTick tick)
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
