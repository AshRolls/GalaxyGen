using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Engine.Controllers;

namespace GalaxyGenEngine.Engine
{
    public class TickEngine : ITickEngine
    {
        bool engineInitialised = false;
        ActorSystem _galaxyActorSystem;
        IActorRef _actorTECoordinator;
        TextOutputController _textOutput;

        public void SetupTickEngine(IGalaxyViewModel state, ITextOutputViewModel textOutput)
        {
            _galaxyActorSystem = ActorSystem.Create("GalaxyActors");

            Props textOutputProps = Props.Create<ActorTextOutput>(textOutput).WithDispatcher("akka.actor.synchronized-dispatcher");
            IActorRef _actorTextOutput = _galaxyActorSystem.ActorOf(textOutputProps, "TextOutput");
            _textOutput = new TextOutputController(_actorTextOutput);

            Props teCoordinatorProps = Props.Create<ActorTickEngineCoordinator>(_textOutput, state.Model);
            _actorTECoordinator = _galaxyActorSystem.ActorOf(teCoordinatorProps, "TECoordinator");
            
            engineInitialised = true;
        }

        public void Run(EngineRunCommand cmd)
        {
            if (!engineInitialised) throw new Exception("You must initialise engine first");
            _actorTECoordinator.Tell(new MessageEngineRunCommand(cmd));
        }

        public void Stop()
        {
            if (!engineInitialised) throw new Exception("You must initialise engine first");

            MessageEngineRunCommand stop = new MessageEngineRunCommand(EngineRunCommand.Stop);
            _actorTECoordinator.Tell(stop);

        }
    }
}
