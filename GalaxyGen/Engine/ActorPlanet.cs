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

            _actorTextOutput.Tell("Planet initialised : " + _planet.Name);            
        }

        private void receiveProducedRes(MessageProducedResources msg)
        {
            Store s = _planet.Stores.Where(x => x.Owner == msg.Owner).FirstOrDefault();
            if (s == null)
            {
                s = new Store();
                s.Location = _planet;
                s.Owner = msg.Owner;
                _planet.Stores.Add(s);
            }

            addResourceQuantityToStore(s, msg);
        }

        private void addResourceQuantityToStore(Store s, MessageProducedResources msg)
        {
            foreach (ResourceQuantity resQ in msg.Resources)
            {
                ResourceQuantity storedResQ = s.StoredResources.Where(x => x.Type == resQ.Type).FirstOrDefault();
                if (storedResQ != null)
                {
                    storedResQ.Quantity += resQ.Quantity;
                }
                else
                {
                    ResourceQuantity newResQ = new ResourceQuantity();
                    newResQ.Quantity = resQ.Quantity;
                    newResQ.Type = resQ.Type;
                    s.StoredResources.Add(newResQ);
                }
            }
        }

        private void receiveTick(MessageTick tick)
        {
            //_actorTextOutput.Tell("TICK RCV P: " + _planetVm.Name + " " + tick.Tick.ToString());

            foreach (IActorRef prodActor in _subscribedActorProducers)
            {
                prodActor.Tell(tick);
            }
        }



    }
}
