using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;

namespace GalaxyGen.Engine
{
    public class TickEngine : ITickEngine
    {
        IGalaxyViewModel _state;
        ActorSystem humanActorSystem;

        public void SetupTickEngine(IGalaxyViewModel state)
        {
            _state = state;

            setupAgentsAsActors();        
        }

        private void setupAgentsAsActors()
        {
            humanActorSystem = ActorSystem.Create("GalaxyActors");
            
            Parallel.ForEach(_state.Agents, (agentVm) =>
            {
                IActorRef actor = humanActorSystem.ActorOf<ActorHuman>(agentVm.Model.AgentId.ToString());
                ActorInitialiseMessage msg = new ActorInitialiseMessage(_state.CurrentTick, agentVm);
                actor.Tell(msg);
            });
        }

        public void RunNTick(int numberOfTicks)
        {
            if (_state == null) throw new Exception("You must initialise engine first");

            for (int i=0;i<=numberOfTicks;i++)
            {
                _state.SolarSystems.First().Planets.First().Name = "Earth 2";
            }
        }
    }
}
