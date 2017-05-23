﻿using GalaxyGen.Model;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class AgentControllerState
    {
        // TRY AND AVOID STORING STATE IN THIS CLASS WHEREEVER POSSIBLE
        private Agent _model;        

        public AgentControllerState(Agent ag)
        {
            if (ag == null) throw new Exception("Agent must not be null");
            _model = ag;
        }

        public String Memory
        {
            get
            {
                return _model.Memory;
            }
            set
            {
                _model.Memory = value;
            }
        }

        public IEnumerable<ScPlanet> PlanetsInSolarSystem
        {
            get
            {
                return StarChart.GetSolarSystem(_model.SolarSystem.StarChartId).Planets;
            }
        }


        public IEnumerable<Int64> PlanetsInSolarSystemScIds
        {
            get
            {
                return _model.SolarSystem.Planets.Select(x => x.StarChartId);
            }
        }

        public AgentStateEnum AgentState
        {
            get
            {
                return _model.AgentState;
            }
        }

        public bool IsPilotingShip
        {
            get
            {
                return _model.AgentState == AgentStateEnum.PilotingShip;
            }
        }

        public ShipStateEnum CurrentShipState
        {
            get
            {
                if (_model.AgentState == AgentStateEnum.PilotingShip)
                {
                    return ((Ship)_model.Location).ShipState;
                }
                else
                {
                    return ShipStateEnum.Unpiloted;
                }
            }
        }
        public bool CurrentShipIsDocked
        {
            get
            {
                return CurrentShipState == ShipStateEnum.Docked;
            }
        }


        private Int64 lastDestinationScId;
        private Planet destinationPlanet;
        private void updateCachedPlanet(Int64 destinationScId)
        {
            if (destinationScId != lastDestinationScId) // if we have a new destination, cache the planet so we only need to look it up once.
            {
                destinationPlanet = _model.SolarSystem.Planets.Where(x => x.StarChartId == destinationScId).FirstOrDefault();
                lastDestinationScId = destinationScId;
            }
        }

        public Double DestinationX(Int64 destinationScId)
        {
            updateCachedPlanet(destinationScId);
            return destinationPlanet.PositionX;            
        }

        public Double DestinationY(Int64 destinationScId)
        {
            updateCachedPlanet(destinationScId);
            return destinationPlanet.PositionY;
        }

        public bool CurrentShipAtDestination(Int64 destinationScId)
        {
            Ship s = (Ship)_model.Location;
            updateCachedPlanet(destinationScId);

            if (destinationPlanet != null && s.PositionX == destinationPlanet.PositionX && s.PositionY == destinationPlanet.PositionY)
            {
                return true;
            }
            return false;
        }
        
        public Int64 CurrentShipId
        {
            get
            {
                return ((Ship)_model.Location).ShipId;
            }
        }

        public Int64 CurrentShipDockedPlanetScId
        {
            get
            {
                Ship s = (Ship)_model.Location;
                return s.DockedPlanet.StarChartId;
            }
        }

        public Double CurrentShipCruisingSpeed
        {
            get
            {
                return ((Ship)_model.Location).Type.MaxCruisingSpeedKmH;
            }
        }

        public Double CurrentShipX
        {
            get
            {
                return ((Ship)_model.Location).PositionX;
            }
        }


        public Double CurrentShipY
        {
            get
            {
                return ((Ship)_model.Location).PositionY;
            }
        }
    }
}
