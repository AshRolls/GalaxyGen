using Akka.Actor;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Framework;
using GalaxyGenEngine.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Controllers
{
    public class SolarSystemController
    {
        private SolarSystem _model;
        private ulong _curTick;
        private Dictionary<UInt64, PlanetController> _planetCs;      
        private Dictionary<UInt64, ShipController> _shipCs;
        private TextOutputController _textOutput;
        private ActorSolarSystem _parentActor;

        public SolarSystemController(SolarSystem ss, ActorSolarSystem parentActor, TextOutputController textOutput)
        {
            _model = ss;
            _parentActor = parentActor;
            _textOutput = textOutput;

            // create child controller for each planet in ss
            _planetCs = new Dictionary<UInt64, PlanetController>();
            foreach (Planet p in ss.Planets.Values)
            {
                PlanetController pc = new PlanetController(p, this, textOutput);
                _planetCs.Add(p.StarChartId, pc);
            }

            // create child controller for each ship in ss
            _shipCs = new Dictionary<UInt64, ShipController>();
            foreach (Ship s in ss.Ships.Values)
            {
                ShipController sc = new ShipController(s, this, textOutput);
                _shipCs.Add(s.ShipId, sc);
            }
        }

        public void Tick(MessageTick tick)
        {                   
            _curTick = tick.Tick;
            updatePlanets(tick);
            updateShips(tick);
        }
        
        private void updatePlanets(MessageTick tick)
        {
            foreach (PlanetController pc in _planetCs.Values)
            {
                pc.Tick(tick);
            }
        }

        private void updateShips(MessageTick tick)
        {
            foreach (ShipController sc in _shipCs.Values)
            {
                sc.Tick(tick);
            }
        }

        // TODO move this into ShipController
        internal void ReceiveCommandForShip(MessageShipCommand msg)
        {           
            ShipController sc = _shipCs[msg.ShipId];
            // check the ship *could* execute this command
            bool success;
            switch (msg.Command.CommandType)
            {
                case ShipCommandEnum.SetXY:
                    success = ShipSetXY(msg, sc);
                    break;
                case ShipCommandEnum.Undock:
                    success = ShipUndock(msg, sc);
                    break;
                case ShipCommandEnum.Dock:
                    success = ShipDock(msg, sc);
                    break;
                case ShipCommandEnum.SetDestination:
                    success = ShipSetDestination(msg, sc);
                    break;
                case ShipCommandEnum.SetAutopilot:
                    success = ShipSetAutopilot(msg, sc);
                    break;                
                default:
                    success = false;
                    throw new Exception("Unknown Ship Command");
            }
            if (!success)
            {
                SendMessageToAgent(msg.AgentId, new MessageAgentCommand(new MessageAgentFailedCommand(AgentCommandEnum.ShipCommandFailed), _curTick));
            }
        }
       
        private bool ShipUndock(MessageShipCommand msg, ShipController sc)
        {
            bool success = false;
            if (sc.checkValidUndockCommand(msg))
            {
                _planetCs[sc.DockedPlanet.StarChartId].UndockShip(msg.ShipId);
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
                Ship s = _model.Ships[msg.ShipId];
                Planet p = _model.Planets[((MessageShipDocking)msg.Command).DockingTargetId];
                _planetCs[p.StarChartId].DockShip(s);                
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
            if (!_planetCs[msg.PlanetScId].CommandForMarket(msg))
            {
                SendMessageToAgent(msg.AgentId, new MessageAgentCommand(new MessageAgentFailedCommand(AgentCommandEnum.MarketCommandFailed), msg.TickSent));
            }
        }

        internal void ReceiveCommandForPlanet(MessagePlanetCommand msg)
        {            
            _planetCs[msg.PlanetScId].CommandForPlanet(msg);
        }

        internal UInt64 SolarSystemId
        {
            get
            {
                return _model.SolarSystemId;
            }
        }

        internal void SendMessageToAgent(UInt64 agentId, object msg)
        {
            _parentActor.SendMessageToAgent(agentId, msg);
        }

        internal bool CurrencyRequest(ulong currencyId, long qty, ulong agentId, ulong tick)
        {
            if (!(_model.Galaxy.Accounts[agentId].Balances[currencyId] >= qty)) return false;
            _model.Galaxy.Accounts[agentId].Balances[currencyId] -= qty;
            return true;
        }

        internal void AddCurrency(ulong currencyId, long qty, ulong agentId)
        {
            _model.Galaxy.Accounts[agentId].Balances[currencyId] += qty;
        }
    }
}
