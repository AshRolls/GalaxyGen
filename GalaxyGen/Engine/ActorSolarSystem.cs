﻿using Akka.Actor;
using GalaxyGen.Engine.Controllers;
using GalaxyGen.Engine.Messages;
using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyGen.Engine
{
    public class ActorSolarSystem : ReceiveActor
    {
        IActorRef _actorEngine;
        IActorRef _actorTextOutput;
        private SolarSystemController _solarSystemC;
        private Dictionary<Int64, IActorRef> _subscribedActorAgents;
        private IEnumerable<IActorRef> _actorAgentValues;
        private Int64 _curTick;
        private int _numberOfIncompleteAg;   

        public ActorSolarSystem(IActorRef actorEngine, IActorRef actorTextOutput, SolarSystem ss)
        {
            _actorEngine = actorEngine;
            _actorTextOutput = actorTextOutput;
            ss.Actor = Self;
            _solarSystemC = new SolarSystemController(ss, this, actorTextOutput);

            setupChildAgentActors(ss);

            Receive<MessageTick>(msg => receiveTick(msg));
            Receive<MessageShipCommand>(msg => receiveCommandForShip(msg));
            Receive<MessageEngineAgCompletedCommand>(msg => receiveAgentCompletedMessage(msg));

            //_actorTextOutput.Tell("Solar System initialised : " + _solarSystem.Name);            
        }

        private void setupChildAgentActors(SolarSystem ss)
        {
            // create child actors for each agent in ss
            _subscribedActorAgents = new Dictionary<Int64, IActorRef>();
            _numberOfIncompleteAg = ss.Agents.Count();
            foreach (Agent agent in ss.Agents)
            {
                Props agentProps = Props.Create<ActorAgent>(_actorTextOutput, agent, Self);
                IActorRef actor = Context.ActorOf(agentProps, "Agent" + agent.AgentId.ToString());
                _subscribedActorAgents.Add(agent.AgentId, actor);
            }
            _actorAgentValues = _subscribedActorAgents.Values;
        }

        private void receiveTick(MessageTick tick)
        {
            _curTick = tick.Tick;
            _solarSystemC.Tick(tick);   
            foreach(IActorRef agentActor in _actorAgentValues)
            {
                agentActor.Tell(tick);
            }
            if (!_subscribedActorAgents.Any())
                sendSSCompletedMessage();        
        }

        private void receiveAgentCompletedMessage(MessageEngineAgCompletedCommand msg)
        {
            if (msg.Tick == _curTick)
            {
                _numberOfIncompleteAg--;
                if (_numberOfIncompleteAg <= 0)
                {
                    _numberOfIncompleteAg = _subscribedActorAgents.Count();
                    sendSSCompletedMessage();
                }
            }
        }

        private void sendSSCompletedMessage()
        {
            MessageEngineSSCompletedCommand tickCompleteCmd = new MessageEngineSSCompletedCommand(_solarSystemC.SolarSystemId, _curTick);
            _actorEngine.Tell(tickCompleteCmd);
        }
        
        private void receiveCommandForShip(MessageShipCommand msg)
        {
            bool success = _solarSystemC.ReceiveCommandForShip(msg);
            MessageShipResponse msr = new MessageShipResponse(success, msg, _curTick);
            Sender.Tell(msr);
        }

        internal void SendMessageToAgent(Int64 agentId, object msg)
        {
            _subscribedActorAgents[agentId].Tell(msg);
        }
    }
}
