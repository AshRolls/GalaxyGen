using Akka.Actor;
using GalaxyGen.Engine.Controllers;
using GalaxyGen.Engine.Controllers.AgentDefault;
using GalaxyGen.Engine.Messages;
using GalaxyGen.Model;
using System;
using System.Linq;

namespace GalaxyGen.Engine
{
    public class ActorMarket : ReceiveActor
    {
        private IActorRef _actorTextOutput;
        private IActorRef _actorSolarSystem;
        private Market _market;
        //private IMarketController _agentC;        

        public ActorMarket (IActorRef actorTextOutput, Market m, IActorRef actorSolarSystem)
        {
            _actorTextOutput = actorTextOutput;
            _actorSolarSystem = actorSolarSystem;
            _market = m;
        }
    }
}
