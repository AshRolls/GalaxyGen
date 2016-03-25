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
    public class ActorAgent : ReceiveActor
    {
        IActorRef _actorTextOutput;
        IActorRef _actorSolarSystem;
        Agent _agent;

        public ActorAgent(IActorRef actorTextOutput, Agent ag, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _agent = ag;
            _agent.Actor = Self;
            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Agent initialised : " + _agent.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            //_actorTextOutput.Tell("TICK RCV H: " + _agentVm.Name + " " + tick.Tick.ToString());
        }

    }
}
