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
            MarketLastCheckedTick = new Dictionary<Int64, Int64>();  // key = planetscid, value = tick
        }

        public Int64 CurrentDestinationScId { get; set; }
        public Dictionary<Int64,Int64> MarketLastCheckedTick { get; set; }
    }
}
