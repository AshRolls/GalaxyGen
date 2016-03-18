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
        ITextOutputViewModel _textOutput;
        ActorSystem _galaxyActorSystem;
        IActorRef _actorTECoordinator;
        IActorRef _actorTextOutput;

        public void SetupTickEngine(IGalaxyViewModel state, ITextOutputViewModel textOutput)
        {
            _state = state;
            _textOutput = textOutput;

            setupAgentsAsActors();        
        }

        private void setupAgentsAsActors()
        {
            _galaxyActorSystem = ActorSystem.Create("GalaxyActors");

            Props textOutputProps = Props.Create<ActorTextOutput>(_textOutput).WithDispatcher("akka.actor.synchronized-dispatcher");          
            _actorTextOutput = _galaxyActorSystem.ActorOf(textOutputProps,"TextOutput");

            Props teCoordinatorProps = Props.Create<ActorTickEngineCoordinator>(_actorTextOutput);
            _actorTECoordinator = _galaxyActorSystem.ActorOf(teCoordinatorProps,"TECoordinator");

            // tell co-ordinator actor to create child actors for each agent
            foreach (IAgentViewModel agentVm in _state.Agents)
            {                
                MessageActorHumanInitialise msg = new MessageActorHumanInitialise(agentVm);
                _actorTECoordinator.Tell(msg);
            }
        }

        public void RunNTick(int numberOfTicks)
        {
            if (_state == null) throw new Exception("You must initialise engine first");

            for (int i=0;i<=numberOfTicks-1;i++)
            {
                _actorTextOutput.Tell("Tick " + i.ToString());
            }
        }
    }
}
