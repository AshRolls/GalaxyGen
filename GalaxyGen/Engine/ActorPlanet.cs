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
        IPlanetViewModel _planetVm;
        IActorRef _actorSolarSystem;
        private HashSet<IActorRef> _subscribedActorProducers;

        public ActorPlanet(IActorRef actorTextOutput, IPlanetViewModel planetVm, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _planetVm = planetVm;
            _subscribedActorProducers = new HashSet<IActorRef>();

            // create child actors for each producer in planet
            foreach (IProducerViewModel prodVm in _planetVm.Producers)
            {
                Props prodProps = Props.Create<ActorProducer>(_actorTextOutput, prodVm, Self);
                IActorRef actor = Context.ActorOf(prodProps, "Producer" + prodVm.Model.ProducerId.ToString());
                _subscribedActorProducers.Add(actor);
            }

            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Planet initialised : " + _planetVm.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            _actorTextOutput.Tell("TICK RCV P: " + _planetVm.Name + " " + tick.Tick.ToString());

            foreach (IActorRef prodActor in _subscribedActorProducers)
            {
                prodActor.Tell(tick);
            }
        }

    }
}
