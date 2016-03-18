using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class MessageActorHumanInitialise
    {
        public MessageActorHumanInitialise(IAgentViewModel agent)
        {
            Agent = agent;
        }

        public IAgentViewModel Agent { get; private set; }
    }
}
