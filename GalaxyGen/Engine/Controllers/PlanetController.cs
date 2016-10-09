using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class PlanetController
    {
        private Planet _model;
        private HashSet<ProducerController> _producerCs;

        public PlanetController(Planet p)
        {
            _model = p;
            _producerCs = new HashSet<ProducerController>();
            // create child controllers for each producer in planet
            foreach (Producer prod in p.Producers)
            {
                ProducerController pc = new ProducerController(prod, this);
                _producerCs.Add(pc);
            }
        }

        public void Tick(MessageTick tick)
        {
            updateProducers(tick);
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
            addResourceQuantityToStore(s, mpr);
        }

        private Store getOrCreateStoreForOwner(Agent owner)
        {
            Store s = getStoreForOwner(owner);
            if (s == null)
            {
                s = new Store();
                s.Location = _model;
                s.Owner = owner;
                _model.Stores.Add(owner.AgentId, s);
            }
            return s;
        }

        private Store getStoreForOwner(Agent owner)
        {
            return _model.Stores[owner.AgentId];
        }

        private void addResourceQuantityToStore(Store s, MessageProducedResources msg)
        {
            foreach (ResourceQuantity resQ in msg.Resources)
            {
                if (s.StoredResources.ContainsKey(resQ.Type))
                {
                    s.StoredResources[resQ.Type] += resQ.Quantity;
                }
                else
                {
                    s.StoredResources.Add(resQ.Type, resQ.Quantity);
                }
            }
        }

        private UInt64 getStoredResourceQtyFromStore(Store s, ResourceTypeEnum type)
        {
            return s.StoredResources[type];
        }

        internal bool ReceiveResourceRequest(MessageRequestResources msg)
        {
            Store s = getStoreForOwner(msg.Owner);
            if (s == null) return false; // does not have resources
                                         // check that we have the resources in store
            bool hasResources = checkResourcesAvailable(msg, s);

            if (hasResources)
            {
                // remove resources
                foreach (ResourceQuantity resQ in msg.ResourcesRequested)
                {
                    s.StoredResources[resQ.Type] -= resQ.Quantity;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkResourcesAvailable(MessageRequestResources msg, Store s)
        {
            bool hasResources = true;
            foreach (ResourceQuantity resQ in msg.ResourcesRequested)
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
    }
}
