﻿using Akka.Actor;
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
            _producerVm.Actor = Self;

            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Producer initialised : " + _producerVm.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            _producerVm.TicksRemaining--;
            if (_producerVm.TicksRemaining <= 0)
            {
                BluePrint bp = BluePrints.GetBluePrint(_producerVm.BluePrintType);
                _producerVm.TicksRemaining = bp.BaseTicks;                  // reset ticks remaining counter
                foreach (ResourceQuantity resQ in bp.Produces)
                {
                    // add res to owners store at producer location
                    _actorTextOutput.Tell(_producerVm.Name + " PRODUCES " + resQ.Quantity + " " + resQ.Type.ToString() + " " + tick.Tick.ToString());
                }
            }
            //_actorTextOutput.Tell("TICK RCV PROD: " + _producerVm.Name + " " + tick.Tick.ToString());
        }

    }
}
