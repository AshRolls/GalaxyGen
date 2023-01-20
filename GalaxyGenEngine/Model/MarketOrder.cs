using GalaxyGenCore.Resources;
using GalaxyGenEngine.Framework;
using System;

namespace GalaxyGenEngine.Model
{
    public class MarketOrder
    {
        public MarketOrder() 
        {
            MarketOrderId = IdUtils.GetId();
        }

        public UInt64 MarketOrderId { get; private set; }
        public bool Buy { get; set; }
        public ResourceTypeEnum Type {get; set;}
        public Int64 Quantity { get; set; }
        public Int64 LimitPrice { get; set; }
        public UInt64 EntryTick { get; set; }
        public UInt64 EventTick { get; set; }

        // TODO AgentId for now, should be Account later
        public UInt64 OwnerId { get; set; }      
    }
}