﻿using Akka.Actor;
using GalaxyGen.Engine.Messages;
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

        }

        internal bool checkValidUndockCommand(MessageShipCommand msg)
        {            
            if (msg.Command.CommandType == ShipCommandEnum.Undock && _model.DockedPlanet != null && _model.ShipState == ShipStateEnum.Docked)
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
            _model.ShipState = ShipStateEnum.Cruising;           
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

        internal void SetDestination(long destinationScId)
        {            
            _model.DestinationScId = destinationScId;
        }
    }
}
