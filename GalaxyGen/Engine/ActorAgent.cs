using Akka.Actor;
using GalaxyGen.Engine.Controllers;
using GalaxyGen.Engine.Controllers.AgentDefault;
using GalaxyGen.Engine.Messages;
using GalaxyGen.Model;
using System;
using System.Linq;

namespace GalaxyGen.Engine
{
    // AGENT SHOULD NEVER CHANGE IT'S OWN MODEL STATE. IT SHOULD TELL SOLARSYSTEM AND THAT WILL MANAGE THE STATE CHANGES.    

    public class ActorAgent : ReceiveActor
    {
        IActorRef _actorTextOutput;
        IActorRef _actorSolarSystem;
        Agent _agent;
        IAgentController _agentC;

        public ActorAgent(IActorRef actorTextOutput, Agent ag, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _agent = ag;
            _agent.SolarSystem.Planets.First().Name = "blah";

            AgentControllerState stateForAgent = new AgentControllerState(ag);
            
            switch(_agent.Type)
            {
                //case AgentTypeEnum.Trader:
                //    _agentC = new AgentTraderController(ag, _actorTextOutput);
                //    break;
                default:
                    _agentC = new AgentDefaultController(stateForAgent, _actorTextOutput);
                    break;
            }

            Receive<MessageTick>(msg => receiveDefaultTick(msg));
            Receive<MessageShipResponse>(msg => receiveShipResponse(msg));
            Receive<MessageAgentDestinationReached>(msg => receiveShipDestinationReached(msg));
        }

        private void receiveDefaultTick(MessageTick tick)
        {
            Object message = _agentC.Tick(tick);
            if (message != null)
                Sender.Tell(message);
            sendAgentCompletedMessage(tick);
        }

        private void receiveShipResponse(MessageShipResponse msg)
        {
            _agentC.ReceiveShipResponse(msg);
        }

        private void receiveShipDestinationReached(MessageAgentDestinationReached msg)
        {
            Object message = _agentC.ReceiveShipDestinationReached(msg);
            if (message != null)
                Sender.Tell(message);
        }

        private void sendAgentCompletedMessage(MessageTick msg)
        {
            MessageEngineAgCompletedCommand tickCompleteCmd = new MessageEngineAgCompletedCommand(_agent.AgentId, msg.Tick);
            _actorSolarSystem.Tell(tickCompleteCmd);
        }


    }
}
