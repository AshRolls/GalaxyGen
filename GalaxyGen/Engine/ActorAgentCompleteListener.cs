using Akka.Actor;
using GalaxyGen.Engine.Controllers;
using GalaxyGen.Engine.Controllers.AgentDefault;
using GalaxyGen.Engine.Messages;
using GalaxyGen.Model;
using System;
using System.Linq;

namespace GalaxyGen.Engine
{    
    public class ActorAgentCompleteListener : ReceiveActor
    {
        IActorRef _actorEngine;
        private int _numberOfAgents;
        private int _numberOfIncompleteAg;
        private Int64 _ssScId;
        private Int64 _curTick;

        public ActorAgentCompleteListener(IActorRef actorEngine, int numberOfAgents, Int64 ssScId, Int64 startingTick)
        {
            _actorEngine = actorEngine;
            _numberOfAgents = numberOfAgents;
            _numberOfIncompleteAg = _numberOfAgents;
            _ssScId = ssScId;
            _curTick = startingTick;

            Receive<MessageEngineAgCompletedCommand>(msg => receiveAgentCompletedMessage(msg));
        }

        private void receiveAgentCompletedMessage(MessageEngineAgCompletedCommand msg)
        {
            _numberOfIncompleteAg--;
            if (_numberOfIncompleteAg <= 0)
            {
                _numberOfIncompleteAg = _numberOfAgents;
                _curTick++;
                sendSSCompletedMessage();
            }
        }

        private void sendSSCompletedMessage()
        {
            MessageEngineSSCompletedCommand tickCompleteCmd = new MessageEngineSSCompletedCommand(_ssScId, _curTick);
            _actorEngine.Tell(tickCompleteCmd);
        }

    }
}
