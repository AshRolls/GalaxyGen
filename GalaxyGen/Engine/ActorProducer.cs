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
    public class ActorProducer : ReceiveActor
    {
        IActorRef _actorTextOutput;
        IProducerViewModel _producerVm;
        IActorRef _actorPlanet;

        public ActorProducer(IActorRef actorTextOutput, IProducerViewModel producerVm, IActorRef actorPlanet)
        {
            _actorTextOutput = actorTextOutput;
            _actorPlanet = actorPlanet;
            _producerVm = producerVm;

            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Producer initialised : " + _producerVm.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            _producerVm.TicksCompleted++;            
            _actorTextOutput.Tell("TICK RCV PROD: " + _producerVm.Name + " " + tick.Tick.ToString());
        }

    }
}
