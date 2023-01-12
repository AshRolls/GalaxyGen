using Akka.Actor;
using System;

namespace GalaxyGenEngine.Engine.Controllers
{
    public class TextOutputController
    {
        private IActorRef _actorTextOutput;
        private bool _enabled;
     
        public TextOutputController(IActorRef actorTextOutput)
        {
            _enabled = true;
            _actorTextOutput = actorTextOutput;
        }

        public void Write(string text) 
        {
            if (_enabled) _actorTextOutput.Tell(text);
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

    }
}
