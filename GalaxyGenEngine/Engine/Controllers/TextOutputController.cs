using Akka.Actor;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Controllers
{
    public class TextOutputController
    {
        private IActorRef _actorTextOutput;
        private bool _enabled;
        private HashSet<ulong> _allowedId;
     
        public TextOutputController(IActorRef actorTextOutput)
        {
            _enabled = true;
            _actorTextOutput = actorTextOutput;
            _allowedId = new HashSet<ulong>();
        }

        public void Write(ulong id, string text) 
        {
            if (_enabled && _allowedId.Contains(id)) _actorTextOutput.Tell(text);
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void AddAllowedId(ulong id)
        {
            if (!_allowedId.Contains(id)) _allowedId.Add(id);
        }

        public void RemoveAllowedId(ulong id)
        {
            if (_allowedId.Contains(id)) _allowedId.Remove(id);
        }

    }
}
