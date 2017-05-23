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
        
        public ShipController(Ship s, SolarSystemController ssc, IActorRef actorTextOutput)
        {
            _model = s;
            _solarSystemC = ssc;
            _actorTextOutput = actorTextOutput;
        }

        public void Tick(MessageTick tick)
        {
            //if (_model.ShipState == ShipStateEnum.SpaceCruising)
            //{
            //    // move ship towards destination
            //    PointD newPoint = NavigationUtils.GetNewPointForShip(_model.Type.MaxCruisingSpeedKmH, _model.PositionX, _model.PositionY, _destination.PositionX, _destination.PositionY);
            //    _model.PositionX = newPoint.X;
            //    _model.PositionY = newPoint.Y;
            //}
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

        internal void Undock()
        {
            _model.PositionX = _model.DockedPlanet.PositionX;
            _model.PositionY = _model.DockedPlanet.PositionY;
            _model.DockedPlanet = null;
            _model.ShipState = ShipStateEnum.SpaceCruising;           
        }

        internal void Dock(Planet p)
        {
            _model.DockedPlanet = p;
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

        internal void SetXY(Double X, Double Y)
        {
            _model.PositionX = X;
            _model.PositionY = Y;
        }

        //internal Planet GetDestination
        //{            
        //    get { return _destination; }            
        //}
    }
}
