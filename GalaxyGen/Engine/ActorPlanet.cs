using Akka.Actor;
using GalaxyGen.Engine;
using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class ActorPlanet : ReceiveActor
    {
        IActorRef _actorTextOutput;
        Planet _planet;
        IActorRef _actorSolarSystem;
        private HashSet<IActorRef> _subscribedActorProducers;
        Int64 curTick;

        public ActorPlanet(IActorRef actorTextOutput, Planet planet, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _planet = planet;
            _planet.Actor = Self;
            _subscribedActorProducers = new HashSet<IActorRef>();

            // create child actors for each producer in planet
            foreach (Producer prod in _planet.Producers)
            {
                Props prodProps = Props.Create<ActorProducer>(_actorTextOutput, prod, Self);
                IActorRef actor = Context.ActorOf(prodProps, "Producer" + prod.ProducerId.ToString());
                _subscribedActorProducers.Add(actor);
            }

            Receive<MessageTick>(msg => receiveTick(msg));
            Receive<MessageProducedResources>(msg => receiveProducedRes(msg));
            Receive<MessageRequestResources>(msg => receiveRequestRes(msg));

            _actorTextOutput.Tell("Planet initialised : " + _planet.Name);            
        }

        private void receiveProducedRes(MessageProducedResources msg)
        {
            Store s = getOrCreateStoreForOwner(msg.Owner);
            addResourceQuantityToStore(s, msg);
        }

        private void receiveRequestRes(MessageRequestResources msg)
        {
            Store s = getStoreForOwner(msg.Owner);
            if (s == null) return; // does not have resources
            // check that we have the resources in store
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

            if (hasResources)
            {
                // remove resources
                foreach (ResourceQuantity resQ in msg.ResourcesRequested)
                {
                    s.StoredResources[resQ.Type] -= resQ.Quantity;              
                }
                MessageRequestResourcesResponse msgRes = new MessageRequestResourcesResponse(true, curTick);
                Sender.Tell(msgRes);
            }
            else
            {
                MessageRequestResourcesResponse msgRes = new MessageRequestResourcesResponse(false, curTick);
                Sender.Tell(msgRes);
            }
        }

        private Store getOrCreateStoreForOwner(Agent owner)
        {
            Store s = getStoreForOwner(owner);
            if (s == null)
            {
                s = new Store();
                s.Location = _planet;
                s.Owner = owner;
                _planet.Stores.Add(s);
            }
            return s;
        }

        private Store getStoreForOwner(Agent owner)
        {
            return _planet.Stores.Where(x => x.Owner == owner).FirstOrDefault(); // TODO slow, optimise!
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

        private void receiveTick(MessageTick tick)
        {
            //_actorTextOutput.Tell("TICK RCV P: " + _planetVm.Name + " " + tick.Tick.ToString());
            curTick = tick.Tick;

            foreach (IActorRef prodActor in _subscribedActorProducers)
            {
                prodActor.Tell(tick);
            }
        }



    }
}
