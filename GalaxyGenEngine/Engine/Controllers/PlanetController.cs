using Akka.Actor;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Framework;
using GalaxyGenEngine.Model;
using GalaxyGenCore;
using GalaxyGenCore.Framework;
using GalaxyGenCore.Resources;
using GalaxyGenCore.StarChart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Controllers
{
    public class PlanetController
    {
        private Planet _model;
        private SolarSystemController _solarSystemC;
        private HashSet<ProducerController> _producerCs;
        private MarketController _marketController;
        private TextOutputController _textOutput;
        private ScPlanet _scPlanet;
        private double _orbitHours;
        private ulong _curTick;

        public PlanetController(Planet p, SolarSystemController ssc, TextOutputController textOutput)
        {
            _model = p;
            _solarSystemC = ssc;
            _textOutput = textOutput;
            _scPlanet = StarChart.GetPlanet(_model.StarChartId);
            _orbitHours = _scPlanet.OrbitDays * (double)Globals.DAYS_TO_TICKS_FACTOR;

            _producerCs = new HashSet<ProducerController>();
            // create child controllers for each producer in planet
            foreach (Producer prod in p.Producers)
            {
                ProducerController pc = new ProducerController(prod, this, textOutput);
                _producerCs.Add(pc);
            }

            _marketController = new MarketController(p.Market, this, _solarSystemC, textOutput);
        }

        public void Tick(MessageTick tick)
        {
            _curTick = tick.Tick;
            movePlanetXY(tick);
            updateProducers(tick);
        }

        private void movePlanetXY(MessageTick tick)
        {
            PointD pt = OrbitalUtils.CalcPositionFromTick(tick.Tick, _orbitHours, _scPlanet.OrbitKm);
            _model.PositionX = pt.X;
            _model.PositionY = pt.Y;
        }

        private void updateProducers(MessageTick tick)
        {
            foreach (ProducerController pc in _producerCs)
            {
                pc.Tick(tick);
            }            
        }

        internal void ReceiveProducedResource(MessageProducedResources mpr)
        {
            Store s = getOrCreateStoreForOwner(mpr.Owner);
            addResourceQuantityToStore(s, mpr.Resources);
        }

        private Store getOrCreateStoreForOwner(Agent owner)
        {
            Store s = getStoreForOwner(owner.AgentId);
            if (s == null)
            {
                s = new Store();
                s.Location = _model;
                s.Owner = owner;
                _model.Stores.Add(owner.AgentId, s);
            }
            return s;
        }

        private Store getStoreForOwner(ulong ownerId)
        {
            return _model.Stores[ownerId];
        }

        private void addResourceQuantityToStore(Store s, List<ResourceQuantity> resources)
        {
            foreach (ResourceQuantity resQ in resources)
            {
                if (s.StoredResources.ContainsKey(resQ.Type))
                {
                    s.StoredResources[resQ.Type] += resQ.Quantity;
                }
                else
                {
                    s.StoredResources.Add(resQ.Type, resQ.Quantity);
                }

                //_actorTextOutput.Tell("Store added " + resQ.Type + " " + resQ.Quantity);
            }
        }

        private Int64 getStoredResourceQtyFromStore(Store s, ResourceTypeEnum type)
        {
            return s.StoredResources[type];
        }

        internal bool ResourcesRequest(List<ResourceQuantity> resourcesRequested, ulong agentId, ulong tick)
        {            
            Store s = getStoreForOwner(agentId);
            if (s == null) return false;
            if (checkResourcesAvailable(resourcesRequested, s))
            {
                foreach (ResourceQuantity resQ in resourcesRequested)
                {
                    if (!ResourceRequest(resQ, agentId, tick)) return false;
                }
            }
            return true;
        }

        internal bool ResourceRequest(ResourceQuantity resQ, ulong agentId, ulong tick)
        {
            Store s = getStoreForOwner(agentId);
            if (s == null) return false; 
            if (checkResourceAvailable(resQ, s))
            {
                // remove resource
                s.StoredResources[resQ.Type] -= resQ.Quantity;                                
                return true;
            }
            return false;
        }

        internal bool ReceiveResourceLoadShipRequest(MessagePlanetCommand msg)
        {
            MessagePlanetRequestShipResources msd = (MessagePlanetRequestShipResources)msg.Command;
            Store planetS = getStoreForOwner(msd.AgentId);
            Store shipS = _model.DockedShips[msd.ShipId].Stores[msd.AgentId];
            if (msg.Command.CommandType == PlanetCommandEnum.RequestLoadShip) return moveResources(planetS, shipS, msd.ResourcesRequested);
            else if (msg.Command.CommandType == PlanetCommandEnum.RequestUnloadShip) return moveResources(shipS, planetS, msd.ResourcesRequested);
            return false;
        }

        private bool moveResources(Store sourceStore, Store destStore, List<ResourceQuantity> resQs)
        {
            if (sourceStore == null || destStore == null) return false; // does not have resources
            if (checkResourcesAvailable(resQs, sourceStore))
            {
                foreach (ResourceQuantity resQ in resQs)
                {
                    // remove resources
                    sourceStore.StoredResources[resQ.Type] -= resQ.Quantity;
                    // add to ship store
                    addResourceQuantityToStore(destStore, new List<ResourceQuantity>() { resQ });
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkResourceAvailable(ResourceQuantity resQ, Store s)
        {
            if (s.StoredResources.ContainsKey(resQ.Type))
            {
                Int64 storedResQ = getStoredResourceQtyFromStore(s, resQ.Type);
                if (storedResQ < resQ.Quantity)
                {
                    return false;
                }
                else return true;
            }
            return false;
        }

        private bool checkResourcesAvailable(List<ResourceQuantity> resourcesRequested, Store s)
        {
            foreach (ResourceQuantity resQ in resourcesRequested)
            {
                if (!checkResourceAvailable(resQ, s)) return false;
            }
            return true;
        }    

        internal void UndockShip(UInt64 shipId)
        {
            Ship s = _model.DockedShips[shipId];
            if (s != null)
                _model.DockedShips.Remove(shipId);
        }

        internal void DockShip(Ship s)
        {
            _model.DockedShips.Add(s.ShipId, s);
        }

        internal void ReceiveCommandForMarket(MessageMarketCommand msg)
        {
            _marketController.receiveMarketCommand(msg);
        }

        internal void ReceiveCommandForPlanet(MessagePlanetCommand msg)
        {
            bool success;
            ulong agentId;
            switch (msg.Command.CommandType)
            {
                case PlanetCommandEnum.RequestLoadShip:
                    agentId = ((MessagePlanetRequestShipResources)msg.Command).AgentId;
                    success = ReceiveResourceLoadShipRequest(msg);
                    break;
                case PlanetCommandEnum.RequestUnloadShip:
                    agentId = ((MessagePlanetRequestShipResources)msg.Command).AgentId;
                    success = ReceiveResourceLoadShipRequest(msg);
                    break;
                default:
                    success = false;
                    throw new Exception("Unknown Ship Command");
            }
            if (!success)
            {
                _solarSystemC.SendMessageToAgent(agentId, new MessageAgentCommand(new MessageAgentFailedCommand(AgentCommandEnum.PlanetCommandFailed), _curTick));
            }

        }
    }
}
