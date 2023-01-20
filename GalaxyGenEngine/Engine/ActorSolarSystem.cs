using Akka.Actor;
using GalaxyGenEngine.Engine.Controllers;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyGenEngine.Engine
{
    public class ActorSolarSystem : ReceiveActor
    {
        IActorRef _actorEngine;
        private TextOutputController _textOutput;
        private SolarSystemController _solarSystemC;
        private Dictionary<UInt64, IActorRef> _subscribedActorAgents; // key agent id        
        private UInt64 _curTick;
        private int _numberOfIncompleteAg;
        private MessageEngineSSCompletedCommand _tickCompleteCmd;

        public ActorSolarSystem(IActorRef actorEngine, TextOutputController textOutput, SolarSystem ss)
        {
            _actorEngine = actorEngine;
            _textOutput = textOutput;
            ss.Actor = Self;
            _solarSystemC = new SolarSystemController(ss, this, textOutput);
            _tickCompleteCmd = new MessageEngineSSCompletedCommand(_solarSystemC.SolarSystemId);

            setupChildAgentActors(ss);

            Receive<MessageTick>(msg => receiveTick(msg));
            Receive<MessageShipCommand>(msg => receiveCommandForShip(msg));
            Receive<MessageMarketCommand>(msg => receiveCommandForMarket(msg));
            Receive<MessagePlanetCommand>(msg => receiveCommandForPlanet(msg));
            Receive<MessageEngineAgCompletedCommand>(msg => receiveAgentCompletedMessage(msg));

            //_actorTextOutput.Tell("Solar System initialised : " + _solarSystem.Name);            
        }

        private void setupChildAgentActors(SolarSystem ss)
        {
            // create child actors for each agent in ss
            _subscribedActorAgents = new Dictionary<UInt64, IActorRef>();
            _numberOfIncompleteAg = ss.Agents.Count();
            if (ss.Agents.Count > 0) _textOutput.AddAllowedId(ss.Agents.First().AgentId); 
            foreach (Agent agent in ss.Agents)
            {
                Props agentProps = Props.Create<ActorAgent>(_textOutput, agent, Self);
                IActorRef actor = Context.ActorOf(agentProps, "Agent" + agent.AgentId.ToString());
                _subscribedActorAgents.Add(agent.AgentId, actor);
            }
        }

        private void receiveTick(MessageTick tick)
        {
            _curTick = tick.Tick;
            _solarSystemC.Tick(tick);
            if (_subscribedActorAgents.Any())
            {
                foreach (IActorRef agentActor in _subscribedActorAgents.Values) agentActor.Tell(tick);                
            }
            else sendSSCompletedMessage();        
        }

        private void receiveAgentCompletedMessage(MessageEngineAgCompletedCommand msg)
        {
            _numberOfIncompleteAg--;
            if (_numberOfIncompleteAg <= 0)
            {
                _numberOfIncompleteAg = _subscribedActorAgents.Count();
                sendSSCompletedMessage();
            }
        }

        private void sendSSCompletedMessage()
        {
            _actorEngine.Tell(_tickCompleteCmd);
        }
        
        private void receiveCommandForShip(MessageShipCommand msg)
        {
            _solarSystemC.ReceiveCommandForShip(msg);            
        }

        private void receiveCommandForMarket(MessageMarketCommand msg)
        {
            _solarSystemC.ReceiveCommandForMarket(msg);
        }

        private void receiveCommandForPlanet(MessagePlanetCommand msg)
        {
            _solarSystemC.ReceiveCommandForPlanet(msg);
        }

        internal void SendMessageToAgent(UInt64 agentId, object msg)
        {
            _subscribedActorAgents[agentId].Tell(msg);
        }
    }
}
