using Akka.Actor;
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
        private Dictionary<Int64, IActorRef> _subscribedActorAgents; // key agent id
        private IEnumerable<IActorRef> _actorAgentValues;
        private Dictionary<Int64, IActorRef> _subscribedActorMarkets; // key planetScId        
        private Int64 _curTick;
        private int _numberOfIncompleteAg;
        private MessageEngineSSCompletedCommand _tickCompleteCmd;

        public ActorSolarSystem(IActorRef actorEngine, IActorRef actorTextOutput, SolarSystem ss)
        {
            _actorEngine = actorEngine;
            _actorTextOutput = actorTextOutput;
            ss.Actor = Self;
            _solarSystemC = new SolarSystemController(ss, this, actorTextOutput);
            _tickCompleteCmd = new MessageEngineSSCompletedCommand(_solarSystemC.SolarSystemId);

            setupChildMarketActors(ss);
            setupChildAgentActors(ss);

            Receive<MessageTick>(msg => receiveTick(msg));
            Receive<MessageShipCommand>(msg => receiveCommandForShip(msg));
            Receive<MessageEngineAgCompletedCommand>(msg => receiveAgentCompletedMessage(msg));

            //_actorTextOutput.Tell("Solar System initialised : " + _solarSystem.Name);            
        }

        private void setupChildMarketActors(SolarSystem ss)
        {
            _subscribedActorMarkets = new Dictionary<Int64, IActorRef>();
            foreach (Planet p in ss.Planets)
            {
                // TODO only create market actors for planets with an active market
                Props marketProps = Props.Create<ActorMarket>(_actorTextOutput, p.Market, ss.Actor);
                IActorRef actor = Context.ActorOf(marketProps, "Market" + p.StarChartId.ToString());
                _subscribedActorMarkets.Add(p.StarChartId, actor);
            }
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
