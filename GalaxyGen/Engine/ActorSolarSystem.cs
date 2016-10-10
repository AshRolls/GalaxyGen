using Akka.Actor;
using GalaxyGen.Engine;
using GalaxyGen.Framework;
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
        private SolarSystemController _solarSystemC;
        private List<IActorRef> _subscribedActorAgents;
        private Int64 _curTick;
        private MessageEngineSSCompletedCommand _tickCompleteCmd;

        public ActorSolarSystem(IActorRef actorTextOutput, SolarSystem ss)
        {
            _actorTextOutput = actorTextOutput;
            ss.Actor = Self;
            _tickCompleteCmd = new MessageEngineSSCompletedCommand(ss.SolarSystemId);
            _solarSystemC = new SolarSystemController(ss, actorTextOutput);

            // create child actors for each agent in ss
            _subscribedActorAgents = new List<IActorRef>();
            foreach (Agent agent in ss.Agents)
            {
                Props agentProps = Props.Create<ActorAgent>(_actorTextOutput, agent, Self);
                IActorRef actor = Context.ActorOf(agentProps, "Agent" + agent.AgentId.ToString());
                _subscribedActorAgents.Add(actor);
            }

            Receive<MessageTick>(msg => receiveTick(msg));
            Receive<MessageShipCommand>(msg => receiveShipCommand(msg));

           //_actorTextOutput.Tell("Solar System initialised : " + _solarSystem.Name);            
        }

         
        private void receiveTick(MessageTick tick)
        {
            _curTick = tick.Tick;
            _solarSystemC.Tick(tick);   
            foreach(IActorRef agentActor in _subscribedActorAgents)
            {
                agentActor.Tell(tick);
            }
            Sender.Tell(_tickCompleteCmd);
        }

        private void receiveShipCommand(MessageShipCommand msg)
        {
            bool success = _solarSystemC.ReceiveShipCommand(msg);
            MessageShipResponse msr = new MessageShipResponse(success, msg, _curTick);
            Sender.Tell(msr);
        }
    }
}
