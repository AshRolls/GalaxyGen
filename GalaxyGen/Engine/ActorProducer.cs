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
        BluePrint _bp;

        public ActorProducer(IActorRef actorTextOutput, Producer producer, IActorRef actorPlanet)
        {
            _actorTextOutput = actorTextOutput;
            _actorPlanet = actorPlanet;
            _producer = producer;
            _producer.Actor = Self;
            _actorOwner = _producer.Owner.Actor;
            _bp = BluePrints.GetBluePrint(_producer.BluePrintType);

            if (_producer.Producing) Producing();
            else NotProducing();
           
            _actorTextOutput.Tell("Producer initialised : " + _producer.Name);            
        }

        private void Producing()
        {
            Receive<MessageTick>(msg => receiveProducingTick(msg));
            Receive<MessageResources>(msg => receiveResourcesError(msg));
        }

        private void NotProducing()
        {
            Receive<MessageTick>(msg => receiveNotProducingTick(msg));
            Receive<MessageResources>(msg => receiveResources(msg));
        }

        private void receiveProducingTick(MessageTick tick)
        {
            if (_producer.TickForNextProduction <= tick.Tick)
            {                
                if (_producer.Owner != null)
                {
                    MessageProducedResources mpr = new MessageProducedResources(_bp.Produces, _producer.Owner);
                    _actorPlanet.Tell(mpr);
                    _producer.Producing = false;
                    //foreach (ResourceQuantity resQ in _bp.Produces)
                    //{
                    //    _actorTextOutput.Tell(_producer.Name + " PRODUCES " + resQ.Quantity + " " + resQ.Type.ToString() + " " + tick.Tick.ToString());
                    //}
                }                

                // assume we always want to continue producing, if we have the resources.
                MessageRequestResources msg = new MessageRequestResources(_bp.Consumes, _producer.Owner, tick.Tick);
                _actorPlanet.Tell(msg);
                Become(NotProducing);
            }            
        }

        private void receiveNotProducingTick(MessageTick tick)
        {
            // check store to see if we have resource bp needs, if so restart
            MessageRequestResources msg = new MessageRequestResources(_bp.Consumes, _producer.Owner, tick.Tick);
            _actorPlanet.Tell(msg);
        }

        private void receiveResources(MessageResources res)
        {            
            _producer.TickForNextProduction = res.TickSent + _bp.BaseTicks;
            _producer.Producing = true;
            Become(Producing);
        }

        private void receiveResourcesError(MessageResources res)
        {
            _actorTextOutput.Tell(_producer.Name + " ERROR, resources received whilst already producing " + res.TickSent.ToString()); 
            //if (_producer.ProducerId == 10)
            //{
            //    Console.WriteLine("error " + res.TickSent.ToString());
            //}
        }

    }
}
