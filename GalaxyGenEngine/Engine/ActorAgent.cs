﻿using Akka.Actor;
using GalaxyGenEngine.Engine.Controllers;
using GalaxyGenEngine.Engine.Controllers.AgentDefault;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyGenEngine.Engine
{
    // AGENT SHOULD NEVER CHANGE IT'S OWN MODEL STATE. IT SHOULD TELL SOLARSYSTEM AND THAT WILL MANAGE THE STATE CHANGES.    

    public class ActorAgent : ReceiveActor
    {
        private IActorRef _actorTextOutput;
        private IActorRef _actorSolarSystem;
        private Agent _agent;
        private IAgentController _agentC;
        private MessageEngineAgCompletedCommand _tickCompleteCmd;

        public ActorAgent(IActorRef actorTextOutput, Agent ag, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
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
                    _agentC = new AgentDefaultController(stateForAgent, _actorSolarSystem, _actorTextOutput);
                    break;
            }

            Receive<MessageTick>(msg => receiveDefaultTick(msg));
        }

        private void receiveDefaultTick(MessageTick tick)
        {
            _agentC.Tick(tick);
            sendAgentCompletedMessage();
        }

        private void sendAgentCompletedMessage()
        {
            _actorSolarSystem.Tell(_tickCompleteCmd);
        }
    }
}
