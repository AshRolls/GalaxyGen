using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class ActorInitialiseMessage
    {
        public ActorInitialiseMessage(Int64 currentTick, IAgentViewModel agent)
        {
            CurrentTick = currentTick;
            Agent = agent;
        }

        public Int64 CurrentTick { get; private set; }
        public IAgentViewModel Agent { get; private set; }
    }
}
