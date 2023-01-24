using Akka.Routing;
using GalaxyGenCore.Resources;
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
            MarketLastCheckedTick = new();  // key = planetscid, value = tick
            StoppedProducers = new();
            SpotPrices = new();
        }

        public UInt64 CurrentDestinationScId { get; set; }
        public Dictionary<ulong, ulong> MarketLastCheckedTick { get; set; }
        public Dictionary<(ulong planetScId, ResourceTypeEnum resT), (long spotPrice, ulong tick)> SpotPrices { get; set; }
        public Dictionary<ulong, (ulong planetScId, List<ResourceQuantity> resQs)> StoppedProducers { get; set; }        
    }
}
