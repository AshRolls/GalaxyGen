using Akka.Actor;
using GalaxyGen.Engine.Messages;
using GalaxyGen.Framework;
using GalaxyGen.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Controllers
{
    public class SolarSystemController
    {
        private SolarSystem _model;
        private Dictionary<Int64, PlanetController> _planetCs;
        private IEnumerable _planetValues;        
        private Dictionary<Int64,ShipController> _shipCs;
        private IEnumerable _shipValues;
        private IActorRef _actorTextOutput;
        private ActorSolarSystem _parentActor;

        public SolarSystemController(SolarSystem ss, ActorSolarSystem parentActor, IActorRef actorTextOutput)
        {
            _model = ss;
            _parentActor = parentActor;
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
            _shipValues = _shipCs.Values;
        }

        public void Tick(MessageTick tick)
        {                       
            updatePlanets(tick);
            updateShips(tick);
        }
        
        private void updatePlanets(MessageTick tick)
        {
            foreach (PlanetController pc in _planetValues)
            {
                pc.Tick(tick);
            }
        }

        private void updateShips(MessageTick tick)
        {
            foreach (ShipController sc in _shipValues)
            {
                sc.Tick(tick);
            }
        }

        internal void ReceiveCommandForShip(MessageShipCommand msg)
        {
           
            ShipController sc = _shipCs[msg.ShipId];
            // check the ship *could* execute this command
       
            switch (msg.Command.CommandType)
            {
                case ShipCommandEnum.SetXY:
                    ShipSetXY(msg, sc);
                    break;
                case ShipCommandEnum.Undock:
                    ShipUndock(msg, sc);
                    break;
                case ShipCommandEnum.Dock:
                    ShipDock(msg, sc);
                    break;
                case ShipCommandEnum.SetDestination:
                    ShipSetDestination(msg, sc);
                    break;
                case ShipCommandEnum.SetAutopilot:
                    ShipSetAutopilot(msg, sc);
                    break;
                default:
                    throw new Exception("Unknown Ship Command");
                    break;
            }

        }
       
        private bool ShipUndock(MessageShipCommand msg, ShipController sc)
        {
            bool success = false;
            if (sc.checkValidUndockCommand(msg))
            {
                _planetCs[sc.DockedPlanet.PlanetId].UndockShip(msg.ShipId);
                sc.Undock();
                success = true;
            }
            return success;
        }

        private bool ShipDock(MessageShipCommand msg, ShipController sc)
        {
            bool success = false;
            if (sc.checkValidDockCommand(msg))
            {
                Ship s = _model.Ships.Where(x => x.ShipId == msg.ShipId).First();
                Planet p = _model.Planets.Where(x => x.StarChartId == ((MessageShipDocking)msg.Command).DockingTargetId).FirstOrDefault();
                _planetCs[p.PlanetId].DockShip(s);                
                sc.Dock(p);
                success = true;
            }
            return success;
        }

        private static bool ShipSetXY(MessageShipCommand msg, ShipController sc)
        {
            bool success = false;
            if (sc.checkValidSetXYCommand(msg))
            {
                MessageShipSetXY msd = (MessageShipSetXY)msg.Command;
                sc.SetXY(msd.X, msd.Y);
                success = true;
            }
            return success;
        }

        private static bool ShipSetDestination(MessageShipCommand msg, ShipController sc)
        {
            bool success = false;
            if (sc.checkValidSetDestinationCommand(msg))
            {
                MessageShipSetDestination msd = (MessageShipSetDestination)msg.Command;
                sc.SetDestination(msd.DestinationScId);
                success = true;
            }
            return success;
        }

        private static bool ShipSetAutopilot(MessageShipCommand msg, ShipController sc)
        {
            bool success = false;
            if (sc.checkValidSetAutopilotCommand(msg))
            {
                MessageShipSetAutopilot msd = (MessageShipSetAutopilot)msg.Command;
                sc.SetAutopilot(msd.Active);
                success = true;
            }
            return success;
        }

        internal void ReceiveCommandForMarket(MessageMarketCommand msg)
        {
            _planetCs[msg.PlanetId].ReceiveCommandForMarket(msg);
        }

        internal Int64 SolarSystemId
        {
            get
            {
                return _model.SolarSystemId;
            }
        }

        internal void SendMessageToAgent(Int64 agentId, object msg)
        {
            _parentActor.SendMessageToAgent(agentId, msg);
        }
    }
}
