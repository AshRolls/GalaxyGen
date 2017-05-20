using GalaxyGen.Model;
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
        private Planet currentDestinationPlanet;
        public bool CurrentShipAtDestination
        {
            get
            {
                if (_model.AgentState == AgentStateEnum.PilotingShip)
                {
                    Ship s = (Ship)_model.Location;
                    if (s.DestinationScId != lastDestinationScId)
                    {
                        currentDestinationPlanet = s.SolarSystem.Planets.Where(x => x.StarChartId == s.DestinationScId).FirstOrDefault();
                        lastDestinationScId = s.DestinationScId;
                    }
                    if (currentDestinationPlanet != null && s.PositionX == currentDestinationPlanet.PositionX && s.PositionY == currentDestinationPlanet.PositionY)
                    {
                        return true;
                    }                  
                }
                return false;
            }
        }

        public Int64 CurrentShipId
        {
            get
            {
                if (_model.AgentState == AgentStateEnum.PilotingShip)
                {
                    return ((Ship)_model.Location).ShipId;
                }
                throw new Exception("Invalid State Query");
            }
        }

        public Int64 CurrentShipDockedPlanetScId
        {
            get
            {
                if (_model.AgentState == AgentStateEnum.PilotingShip)
                {
                    Ship s = (Ship)_model.Location;
                    return s.DockedPlanet.StarChartId;
                }
                throw new Exception("Invalid State Query");
            }
        }

        public Int64 CurrentShipDestinationScId
        {
            get
            {
                if (_model.AgentState == AgentStateEnum.PilotingShip)
                {
                    Ship s = (Ship)_model.Location;
                    return s.DestinationScId;
                }
                throw new Exception("Invalid State Query");
            }
        }
    }
}
