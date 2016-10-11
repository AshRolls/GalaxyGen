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
    // AGENT SHOULD NEVER CHANGE IT'S OWN STATE. IT SHOULD TELL SOLARSYSTEM AND THAT WILL MANAGE THE STATE CHANGES.

    public class ActorAgent : ReceiveActor
    {
        IActorRef _actorTextOutput;
        IActorRef _actorSolarSystem;
        IReadOnlyAgent _agent;
        IAgentController _agentC;

        public ActorAgent(IActorRef actorTextOutput, IReadOnlyAgent ag, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _agent = ag;
            //_agent.Actor = Self;
            
            switch(_agent.Type)
            {
                //case AgentTypeEnum.Trader:
                //    _agentC = new AgentTraderController(ag, _actorTextOutput);
                //    break;
                default:
                    _agentC = new AgentDefaultController(ag, _actorTextOutput);
                    break;
            }

            Receive<MessageTick>(msg => receiveDefaultTick(msg));
        }

        private void receiveDefaultTick(MessageTick tick)
        {
            Object message = _agentC.Tick(tick);
            if (message != null)
                Sender.Tell(message);
            sendAgentCompletedMessage(tick);
        }

        private void sendAgentCompletedMessage(MessageTick msg)
        {
            MessageEngineAgCompletedCommand tickCompleteCmd = new MessageEngineAgCompletedCommand(_agent.AgentId, msg.Tick);
            _actorSolarSystem.Tell(tickCompleteCmd);
        }

    }
}
