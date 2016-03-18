﻿using GalaxyGen.ViewModel;
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
        bool engineInitialised = false;
        ActorSystem _galaxyActorSystem;
        IActorRef _actorTECoordinator;
        IActorRef _actorTextOutput;

        public void SetupTickEngine(IGalaxyViewModel state, ITextOutputViewModel textOutput)
        {
            _galaxyActorSystem = ActorSystem.Create("GalaxyActors");

            Props textOutputProps = Props.Create<ActorTextOutput>(textOutput).WithDispatcher("akka.actor.synchronized-dispatcher");
            _actorTextOutput = _galaxyActorSystem.ActorOf(textOutputProps, "TextOutput");

            Props teCoordinatorProps = Props.Create<ActorTickEngineCoordinator>(_actorTextOutput, state);
            _actorTECoordinator = _galaxyActorSystem.ActorOf(teCoordinatorProps, "TECoordinator");
            
            engineInitialised = true;
        }

        public void Run()
        {
            if (!engineInitialised) throw new Exception("You must initialise engine first");

            MessageEngineRunCommand run = new MessageEngineRunCommand(EngineRunCommand.Run);
            _actorTECoordinator.Tell(run);

        }

        public void Stop()
        {
            if (!engineInitialised) throw new Exception("You must initialise engine first");

            MessageEngineRunCommand stop = new MessageEngineRunCommand(EngineRunCommand.Stop);
            _actorTECoordinator.Tell(stop);

        }
    }
}
