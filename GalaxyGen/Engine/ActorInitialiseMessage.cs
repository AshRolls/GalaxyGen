using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class ActorInitialiseMessage
    {
        public ActorInitialiseMessage(Int64 currentTick)
        {
            CurrentTick = currentTick;
        }

        public Int64 CurrentTick { get; private set; }
    }
}
