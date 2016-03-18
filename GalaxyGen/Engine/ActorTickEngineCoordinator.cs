using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class ActorTickEngineCoordinator : ReceiveActor
    {
        private HashSet<IActorRef> _subscribedActorHumans;
        IActorRef _actorTextOutput;

        public ActorTickEngineCoordinator(IActorRef actorTextOutput)
        {            
            _subscribedActorHumans = new HashSet<IActorRef>();
            _actorTextOutput = actorTextOutput;

            Receive<MessageActorHumanInitialise>(msg => receiveInitialiseMsg(msg));
        }

        private void receiveInitialiseMsg(MessageActorHumanInitialise msg)
        {
            Props humanProps = Props.Create<ActorHuman>(_actorTextOutput);
            IActorRef actor = Context.ActorOf(humanProps, msg.Agent.Model.AgentId.ToString());
            actor.Tell(msg);
            _subscribedActorHumans.Add(actor);            
        }

    }    
}
