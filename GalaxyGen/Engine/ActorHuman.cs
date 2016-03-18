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

        public ActorHuman(IActorRef actorTextOutput)
        {
            _actorTextOutput = actorTextOutput;
            Receive<MessageActorHumanInitialise>(msg => receiveInitialiseMsg(msg));
        }

        private void receiveInitialiseMsg(MessageActorHumanInitialise msg)
        {
            agent = msg.Agent;
            _actorTextOutput.Tell("Human Agent initialised : " + msg.Agent.Name );
        }

        private IAgentViewModel agent
        {
            get; set;
        }

    }
}
