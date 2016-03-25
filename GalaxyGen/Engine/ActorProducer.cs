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
        Producer _producer;
        IActorRef _actorPlanet;
        IActorRef _actorOwner;

        public ActorProducer(IActorRef actorTextOutput, Producer producer, IActorRef actorPlanet)
        {
            _actorTextOutput = actorTextOutput;
            _actorPlanet = actorPlanet;
            _producer = producer;
            _producer.Actor = Self;
            _actorOwner = _producer.Owner.Actor;

            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Producer initialised : " + _producer.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            if (_producer.TickForNextProduction <= tick.Tick)
            {
                BluePrint bp = BluePrints.GetBluePrint(_producer.BluePrintType);
                _producer.TickForNextProduction = tick.Tick + bp.BaseTicks;                  // reset ticks remaining counter

                if (_producer.Owner != null)
                {
                    MessageProducedResources mpr = new MessageProducedResources(bp.Produces, _producer.Owner);
                    _actorPlanet.Tell(mpr);
                }
                else
                {
                    // shut down production
                }

                foreach (ResourceQuantity resQ in bp.Produces)
                {
                    // add res to owners store at producer location
                    _actorTextOutput.Tell(_producer.Name + " PRODUCES " + resQ.Quantity + " " + resQ.Type.ToString() + " " + tick.Tick.ToString());
                }
            }
            //_actorTextOutput.Tell("TICK RCV PROD: " + _producerVm.Name + " " + tick.Tick.ToString());
        }

    }
}
