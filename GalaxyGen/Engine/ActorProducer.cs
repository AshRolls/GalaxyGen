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
    public class ActorProducer : ReceiveActor, IWithUnboundedStash
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
            Receive<MessageRequestResourcesResponse>(msg => receiveResourcesError(msg));
        }

        private void AwaitingResourceReqResponse()
        {
            Receive<MessageTick>(msg => receiveAwaitingTick(msg));
            Receive<MessageRequestResourcesResponse>(msg => receiveResources(msg));
        }

        private void NotProducing()
        {
            Receive<MessageTick>(msg => receiveNotProducingTick(msg));
            Receive<MessageRequestResourcesResponse>(msg => receiveResources(msg));
        }

        public IStash Stash { get; set; }

        private void receiveProducingTick(MessageTick tick)
        {
            if (_producer.TickForNextProduction <= tick.Tick)
            {                
                if (_producer.Owner != null)
                {
                    MessageProducedResources mpr = new MessageProducedResources(_bp.Produces, _producer.Owner);
                    _actorPlanet.Tell(mpr);
                    _producer.Producing = false;
                    foreach (ResourceQuantity resQ in _bp.Produces)
                    {
                        _actorTextOutput.Tell(_producer.Name + " PRODUCES " + resQ.Quantity + " " + resQ.Type.ToString() + " " + tick.Tick.ToString());
                    }
                }

                if (_producer.AutoResumeProduction)
                {
                    requestResources(tick);
                }
                else
                {
                    Become(NotProducing);
                }
            }            
        }

        private void receiveNotProducingTick(MessageTick tick)
        {
            if (_producer.AutoResumeProduction)
            {
                requestResources(tick);
            }
        }

        private void requestResources(MessageTick tick)
        {
            MessageRequestResources msg = new MessageRequestResources(_bp.Consumes, _producer.Owner, tick.Tick);
            _actorPlanet.Tell(msg);
            Become(AwaitingResourceReqResponse);
        }

        // TODO put in system for when we never receive a response!
        private void receiveAwaitingTick(MessageTick tick)
        {
            Stash.Stash(); // stash messages while we are waiting for our response.
        }

        private void receiveResources(MessageRequestResourcesResponse msg)
        {
            if (msg.Response == true)
            {
                _producer.TickForNextProduction = msg.TickSent + _bp.BaseTicks;
                _producer.Producing = true;
                Become(Producing);                
            }
            else
            {
                Become(NotProducing);
            }
            Stash.UnstashAll();
        }

        private void receiveResourcesError(MessageRequestResourcesResponse res)
        {
            _actorTextOutput.Tell(_producer.Name + " ERROR, resources received whilst already producing " + res.TickSent.ToString()); 
            //if (_producer.ProducerId == 10)
            //{
            //    Console.WriteLine("error " + res.TickSent.ToString());
            //}
        }

    }
}
