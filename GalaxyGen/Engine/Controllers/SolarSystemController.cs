using Akka.Actor;
using GalaxyGen.Framework;
using GalaxyGen.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class SolarSystemController
    {
        private SolarSystem _model;
        private Dictionary<Int64, PlanetController> _planetCs;
        private IEnumerable _planetValues;
        private Dictionary<Int64,ShipController> _shipCs;
        private IActorRef _actorTextOutput;             

        public SolarSystemController(SolarSystem ss, IActorRef actorTextOutput)
        {
            _model = ss;                        
            _actorTextOutput = actorTextOutput;

            // create child controller for each planet in ss
            _planetCs = new Dictionary<Int64, PlanetController>();
            foreach (Planet p in ss.Planets)
            {
                PlanetController pc = new PlanetController(p, actorTextOutput);
                _planetCs.Add(p.PlanetId, pc);
            }
            _planetValues = _planetCs.Values;

            // create child controller for each ship in ss
            _shipCs = new Dictionary<Int64, ShipController>();
            foreach (Ship s in ss.Ships)
            {
                ShipController sc = new ShipController(s, this, actorTextOutput);
                _shipCs.Add(s.ShipId, sc);
            }
        }

        public void Tick(MessageTick tick)
        {
            movePlanetXY(tick);            
            updatePlanets(tick);
        }
        
        private void updatePlanets(MessageTick tick)
        {
            foreach (PlanetController pc in _planetValues)
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

        internal bool ReceiveShipCommand(MessageShipCommand msg)
        {
            bool success = false;
            ShipController sc = _shipCs[msg.ShipId];
            // check the ship *could* execute this command
            if (sc.checkValidCommand(msg))
            {
                if (msg.Command == ShipCommandEnum.Undock)
                {
                    _planetCs[sc.DockedPlanet.PlanetId].UndockShip(msg.ShipId);
                    sc.Undock();
                    success = true;
                }                
            }
            return success;
        }

        internal Int64 SolarSystemId
        {
            get
            {
                return _model.SolarSystemId;
            }
        }
    }
}
