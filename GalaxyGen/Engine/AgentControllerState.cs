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
                if (_model.Location != null && _model.Location.GalType == TypeEnum.Ship)
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

        public bool CurrentShipAtDestination
        {
            get
            {
                if (_model.Location != null && _model.Location.GalType == TypeEnum.Ship)
                {
                    Ship s = (Ship)_model.Location;
                    Planet p = s.SolarSystem.Planets.Where(x => x.StarChartId == s.DestinationScId).FirstOrDefault();
                    if (s.PositionX == p.PositionX && s.PositionY == p.PositionY)
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
                if (_model.Location != null && _model.Location.GalType == TypeEnum.Ship)
                {
                    return ((Ship)_model.Location).ShipId;
                }
                return 0;
            }
        }

        public Int64 CurrentShipDockedPlanetScId
        {
            get
            {
                if (_model.Location != null && _model.Location.GalType == TypeEnum.Ship)
                {
                    Ship s = (Ship)_model.Location;
                    if (s.DockedPlanet != null)
                        return s.DockedPlanet.StarChartId;
                }
                return 0;
            }
        }

        public Int64 CurrentShipDestinationScId
        {
            get
            {
                if (_model.Location != null && _model.Location.GalType == TypeEnum.Ship)
                {
                    Ship s = (Ship)_model.Location;                    
                    return s.DestinationScId;
                }
                return 0;
            }
        }
    }
}
