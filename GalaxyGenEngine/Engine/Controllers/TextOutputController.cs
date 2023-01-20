using Akka.Actor;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Controllers
{
    public class TextOutputController
    {
        private IActorRef _actorTextOutput;
        private bool _enabled;
        private ulong _allowedId;
     
        public TextOutputController(IActorRef actorTextOutput)
        {
            _enabled = true;
            _actorTextOutput = actorTextOutput;
            _allowedId = 0;
        }

        public void Write(ulong id, string text) 
        {
            if (_enabled && _allowedId == id) _actorTextOutput.Tell(text);
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void SetAllowedId(ulong id)
        {        
            _allowedId = id;                      
        }
    }
}
