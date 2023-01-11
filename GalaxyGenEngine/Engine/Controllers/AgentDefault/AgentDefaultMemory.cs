using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Controllers.AgentDefault
{
    public class AgentDefaultMemory
    {
        public AgentDefaultMemory()
        {
            MarketLastCheckedTick = new Dictionary<UInt64, UInt64>();  // key = planetscid, value = tick
        }

        public UInt64 CurrentDestinationScId { get; set; }
        public Dictionary<UInt64,UInt64> MarketLastCheckedTick { get; set; }
    }
}
