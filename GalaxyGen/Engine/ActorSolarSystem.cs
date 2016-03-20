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
    public class ActorSolarSystem : ReceiveActor
    {
        IActorRef _actorTextOutput;
        ISolarSystemViewModel _solarSystemVm;
        private HashSet<IActorRef> _subscribedActorHumans;

        public ActorSolarSystem(IActorRef actorTextOutput, ISolarSystemViewModel ssVm)
        {
            _actorTextOutput = actorTextOutput;
            _solarSystemVm = ssVm;
            _subscribedActorHumans = new HashSet<IActorRef>();

            // create child actors for each agent
            foreach (IAgentViewModel agentVm in _solarSystemVm.Agents)
            {
                Props humanProps = Props.Create<ActorHuman>(_actorTextOutput, agentVm);
                IActorRef actor = Context.ActorOf(humanProps, "Human" + agentVm.Model.AgentId.ToString());
                _subscribedActorHumans.Add(actor);
            }

            Receive<MessageTick>(msg => receiveTick(msg));

            _actorTextOutput.Tell("Solar System initialised : " + _solarSystemVm.Name);            
        }

        private void receiveTick(MessageTick tick)
        {
            _actorTextOutput.Tell("TICK RCV SS: " + _solarSystemVm.Name + " " + tick.Tick.ToString());

            foreach (IActorRef humanActor in _subscribedActorHumans)
            {
                humanActor.Tell(tick);
            }
        }

    }
}
