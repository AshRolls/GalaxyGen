using Akka.Actor;
using GalaxyGen.Engine.Messages;
using GalaxyGen.Framework;
using GalaxyGen.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Controllers
{
    public class ShipController
    {
        private Ship _model;
        private SolarSystemController _solarSystemC;
        private IActorRef _actorTextOutput;
        private Planet _destination;
        
        public ShipController(Ship s, SolarSystemController ssc, IActorRef actorTextOutput)
        {
            _model = s;
            _solarSystemC = ssc;
            _actorTextOutput = actorTextOutput;
            _destination = _model.SolarSystem.Planets.Where(x => x.StarChartId == _model.DestinationScId).FirstOrDefault();
        }

        public void Tick(MessageTick tick)
        {
            if (_model.ShipState == ShipStateEnum.SpaceCruising && _model.DestinationScId != 0)
            {
                // move ship towards destination
                PointD newPoint = NavigationUtils.GetNewPointForShip(_model.Type.MaxCruisingSpeedKmH, _model.PositionX, _model.PositionY, _destination.PositionX, _destination.PositionY);
                _model.PositionX = newPoint.X;
                _model.PositionY = newPoint.Y;
                if (_model.PositionX == _destination.PositionX && _model.PositionY == _destination.PositionY)
                {
                    // we are at our destination.
                    MessageAgentDestinationReached msg = new MessageAgentDestinationReached(tick.Tick);
                    _solarSystemC.SendMessageToAgent(_model.Pilot.AgentId, msg);
                }
            }
        }

        internal bool checkValidUndockCommand(MessageShipCommand msg)
        {            
            if (msg.Command.CommandType == ShipCommandEnum.Undock && _model.DockedPlanet != null && _model.ShipState == ShipStateEnum.Docked)
            {
                return true;
            }
            return false;
        }

        internal bool checkValidDockCommand(MessageShipCommand msg)
        {
            if (msg.Command.CommandType == ShipCommandEnum.Dock && _model.DockedPlanet == null && _model.ShipState != ShipStateEnum.Docked)
            {
                return true;
            }
            return false;
        }

        internal bool checkValidSetDestinationCommand(MessageShipCommand msg)
        {
            if (msg.Command.CommandType == ShipCommandEnum.SetDestination)
            {
                return true;
            }
            return false;
        }

        internal void Undock()
        {
            _model.PositionX = _model.DockedPlanet.PositionX;
            _model.PositionY = _model.DockedPlanet.PositionY;
            _model.DockedPlanet = null;
            _model.ShipState = ShipStateEnum.SpaceCruising;           
        }

        internal void Dock()
        {
            _model.DockedPlanet = _destination;
            SetDestination(0);
            _model.ShipState = ShipStateEnum.Docked;
        }

        internal Planet DockedPlanet
        {
            get
            {
                return _model.DockedPlanet;
            }
        }

        private string shipStatus()
        {
            StringBuilder initStr = new StringBuilder();
            initStr.Append(_model.Name);
            initStr.Append(" [");
            if (_model.ShipState == ShipStateEnum.Docked)
            {
                ScPlanet p = StarChart.GetPlanet(_model.DockedPlanet.StarChartId);
                initStr.Append(p.Name);
            }
            else
            {
                initStr.Append(_model.PositionX);
                initStr.Append(",");
                initStr.Append(_model.PositionY);
            }
            initStr.Append("]");
            return initStr.ToString();
        }

        internal void SetDestination(Int64 destinationScId)
        {            
            _model.DestinationScId = destinationScId;
            _destination = _model.SolarSystem.Planets.Where(x => x.StarChartId == destinationScId).FirstOrDefault();
        }

        internal Planet GetDestination
        {            
            get { return _destination; }            
        }
    }
}
