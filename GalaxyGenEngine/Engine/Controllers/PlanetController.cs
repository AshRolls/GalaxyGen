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
        private HashSet<ProducerController> _producerCs;
        private IActorRef _actorTextOutput;
        private ScPlanet _scPlanet;
        private Double _orbitHours;

        public PlanetController(Planet p, IActorRef actorTextOutput)
        {
            _model = p;
            _actorTextOutput = actorTextOutput;
            _scPlanet = StarChart.GetPlanet(_model.StarChartId);
            _orbitHours = _scPlanet.OrbitDays * (double)Globals.DAYS_TO_TICKS_FACTOR;

            _producerCs = new HashSet<ProducerController>();
            // create child controllers for each producer in planet
            foreach (Producer prod in p.Producers)
            {
                ProducerController pc = new ProducerController(prod, this, actorTextOutput);
                _producerCs.Add(pc);
            }

            // TODO create a controller for market.

        }

        public void Tick(MessageTick tick)
        {
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

        private Store getStoreForOwner(long ownerId)
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

        private UInt64 getStoredResourceQtyFromStore(Store s, ResourceTypeEnum type)
        {
            return s.StoredResources[type];
        }

        internal bool ReceiveResourceRequest(MessagePlanetRequestResources msg)
        {            
            Store s = getStoreForOwner(msg.OwnerId);
            if (s == null) return false; // does not have resources
            if (checkResourcesAvailable(msg.ResourcesRequested, s))
            {
                // remove resources
                foreach (ResourceQuantity resQ in msg.ResourcesRequested)
                {
                    s.StoredResources[resQ.Type] -= resQ.Quantity;
                    //_actorTextOutput.Tell("Store removed " + resQ.Type + " " + resQ.Quantity);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool ReceiveResourceLoadShipRequest(MessagePlanetCommand msg)
        {
            MessagePlanetRequestShipResources msd = (MessagePlanetRequestShipResources)msg.Command;
            Store planetS = getStoreForOwner(msd.OwnerId);
            Store shipS = _model.DockedShips[msd.ShipId].Stores[msd.OwnerId];
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

        private bool checkResourcesAvailable(List<ResourceQuantity> resourcesRequested, Store s)
        {
            bool hasResources = true;
            foreach (ResourceQuantity resQ in resourcesRequested)
            {
                if (s.StoredResources.ContainsKey(resQ.Type))
                {
                    UInt64 storedResQ = getStoredResourceQtyFromStore(s, resQ.Type);
                    if (storedResQ < resQ.Quantity)
                    {
                        hasResources = false;
                        break;
                    }
                }
                else
                {
                    hasResources = false;
                    break;
                }
            }

            return hasResources;
        }    

        internal void UndockShip(Int64 shipId)
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
            throw new NotImplementedException();
        }

        internal void ReceiveCommandForPlanet(MessagePlanetCommand msg)
        {
            switch (msg.Command.CommandType)
            {
                case PlanetCommandEnum.RequestLoadShip:
                    ReceiveResourceLoadShipRequest(msg);
                    break;
                case PlanetCommandEnum.RequestUnloadShip:
                    ReceiveResourceLoadShipRequest(msg);
                    break;
                default:
                    throw new Exception("Unknown Ship Command");
            }
        }
    }
}
