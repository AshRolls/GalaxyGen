using Akka.Actor;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Framework;
using GalaxyGenEngine.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Controllers
{
    public class ShipController
    {
        private Ship _model;
        private SolarSystemController _solarSystemC;
        private TextOutputController _textOutput;
        private Planet _destination;        
        public ShipController(Ship s, SolarSystemController ssc, TextOutputController textOutput)
        {
            _model = s;
            _solarSystemC = ssc;
            _textOutput = textOutput;
            if (_model.DestinationScId != 0)
            {
                _destination = _model.SolarSystem.Planets[_model.DestinationScId];
            }
        }

        public void Tick(MessageTick tick)
        {
            if (_model.ShipState == ShipStateEnum.SpaceAutopilot)
            {
                // move ship towards destination
                _model.Position = NavigationUtils.GetNewPointForShip(_model.Type.MaxCruisingSpeedKmH, _model.Position.X, _model.Position.Y, _destination.Position.X, _destination.Position.Y);
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

        internal bool checkValidSetXYCommand(MessageShipCommand msg)
        {
            if (msg.Command.CommandType == ShipCommandEnum.SetXY)
            {
                return true;
            }
            //TODO Check within speed of ship
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

        internal bool checkValidSetAutopilotCommand(MessageShipCommand msg)
        {
            if (msg.Command.CommandType == ShipCommandEnum.SetAutopilot)
            {
                return true;
            }

            // TODO check we have a destination and are in space
            return false;
        }

        internal void Undock()
        {
            _model.Position = _model.DockedPlanet.Position;            
            _model.DockedPlanet = null;
            _model.AutopilotActive = false;
            _model.ShipState = ShipStateEnum.SpaceManual;           
        }

        internal void Dock(Planet p)
        {
            _model.DockedPlanet = p;
            _model.AutopilotActive = false;
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
                initStr.Append(_model.Position.X);
                initStr.Append(",");
                initStr.Append(_model.Position.Y);
            }
            initStr.Append("]");
            return initStr.ToString();
        }

        internal void SetXY(Double X, Double Y)
        {
            _model.Position = new Vector2(X, Y);            
        }

        internal void SetDestination(UInt64 destinationScId)
        {
            _model.DestinationScId = destinationScId;
            _destination = _model.SolarSystem.Planets[destinationScId];
        }

        internal void SetAutopilot(bool active)
        {
            _model.AutopilotActive = active;
            if (active)
                _model.ShipState = ShipStateEnum.SpaceAutopilot;
            else
                _model.ShipState = ShipStateEnum.SpaceManual; 
        }

        internal Planet GetDestination
        {
            get { return _destination; }
        }


    }
}
