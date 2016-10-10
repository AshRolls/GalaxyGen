using Akka.Actor;
using GalaxyGen.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class AgentController
    {
        private Agent _model;
        private IActorRef _actorTextOutput;

        public AgentController(Agent a, IActorRef actorTextOutput)
        {
            _model = a;
            _actorTextOutput = actorTextOutput; 
        }

        public void Tick(MessageTick tick)
        {

        }   
    }
}
