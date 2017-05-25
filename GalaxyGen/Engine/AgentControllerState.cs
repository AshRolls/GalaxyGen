using GalaxyGen.Framework;
using GalaxyGen.Model;
using GalaxyGenCore.Resources;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class AgentControllerState : IAgentControllerState
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

        public bool XYAtDestination(Int64 destinationScId, Double X, Double Y)
        {
            updateCachedPlanet(destinationScId);

            if (destinationPlanet != null && X == destinationPlanet.PositionX && Y == destinationPlanet.PositionY)
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

        public PointD CurrentShipXY
        {
            get
            {
                Ship s = (Ship)_model.Location;
                return new PointD(s.PositionX, s.PositionY);
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

        public bool CurrentShipAutopilotActive
        {
            get
            {
                return ((Ship)_model.Location).AutopilotActive;
            }
        }

        public bool CurrentShipHasDestination
        {
            get
            {
                return ((Ship)_model.Location).DestinationScId != 0;
            }
        }

        public Int64 CurrentShipDestinationScID
        {
            get
            {
                return ((Ship)_model.Location).DestinationScId;
            }
        }

        public UInt64 CurrentShipResourceQuantity(ResourceTypeEnum res)
        {
            Store s = ((Ship)_model.Location).Stores[_model.AgentId];
            if (s.StoredResources.ContainsKey(res))
                return s.StoredResources[res];
            else
                return 0;
        }

        public UInt64 PlanetResourceQuantity(Int64 planetScId, ResourceTypeEnum res)
        {
            Planet p = _model.SolarSystem.Planets.Where(x => x.StarChartId == planetScId).FirstOrDefault();
            Store s = p.Stores[_model.AgentId];
            if (s.StoredResources.ContainsKey(res))
                return s.StoredResources[res];
            else
                return 0;
        }

        public List<ResourceQuantity> PlanetResources(Int64 planetScId)
        {
            Planet p = _model.SolarSystem.Planets.Where(x => x.StarChartId == planetScId).FirstOrDefault();
            Store s = p.Stores[_model.AgentId];
            List<ResourceQuantity> resources = new List<ResourceQuantity>();
            foreach(KeyValuePair<ResourceTypeEnum,UInt64> kvp in s.StoredResources)
            {
                resources.Add(new ResourceQuantity(kvp.Key, kvp.Value));
            }
            return resources;
        }
    }
}
