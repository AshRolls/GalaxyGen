using Akka.Actor;
using GalaxyGenEngine.Engine.Controllers;
using GalaxyGenEngine.Engine.Controllers.AgentDefault;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Akka.Actor.FSMBase;

namespace GalaxyGenEngine.Engine
{
    // AGENT SHOULD NEVER CHANGE IT'S OWN MODEL STATE. IT SHOULD TELL SOLARSYSTEM AND THAT WILL MANAGE THE STATE CHANGES.    

    public class ActorAgent : ReceiveActor
    {
        private TextOutputController _textOutput;
        private IActorRef _actorSolarSystem;
        private Agent _agent;
        private IAgentController _agentC;
        private MessageEngineAgCompletedCommand _tickCompleteCmd;

        public ActorAgent(TextOutputController textOutput, Agent ag, IActorRef actorSolarSystem)
        {
            _textOutput = textOutput;
            _actorSolarSystem = actorSolarSystem;
            _agent = ag;
            _tickCompleteCmd = new MessageEngineAgCompletedCommand(_agent.AgentId);

            AgentControllerState stateForAgent = new AgentControllerState(ag);
            
            switch(_agent.Type)
            {
                //case AgentTypeEnum.Trader:
                //    _agentC = new AgentTraderController(ag, _actorTextOutput);
                //    break;
                default:
                    _agentC = new AgentDefaultController(stateForAgent, _actorSolarSystem, _textOutput);
                    break;
            }

            Receive<MessageTick>(msg => receiveDefaultTick(msg));
            Receive<MessageAgentCommand>(msg => receiveCommand(msg));
        }        

        private void receiveDefaultTick(MessageTick tick)
        {
            //_actorTextOutput.Tell(_agent.AgentId + " agent recv tick");
            _agentC.Tick(tick);
            sendAgentCompletedMessage();
        }

        private void receiveCommand(MessageAgentCommand msg)
        {
            _agentC.ReceiveCommand(msg);
        }

        private void sendAgentCompletedMessage()
        {
            //_actorTextOutput.Tell(_agent.AgentId + " agent complete");
            _actorSolarSystem.Tell(_tickCompleteCmd);            
        }
    }
}
