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
    public class ActorHuman : ReceiveActor
    {
        IActorRef _actorTextOutput;
        IActorRef _actorSolarSystem;
        IAgentViewModel _agentVm;

        public ActorHuman(IActorRef actorTextOutput, IAgentViewModel agentVm, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _agentVm = agentVm;
            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Human Agent initialised : " + _agentVm.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            //_actorTextOutput.Tell("TICK RCV H: " + _agentVm.Name + " " + tick.Tick.ToString());
        }

    }
}
