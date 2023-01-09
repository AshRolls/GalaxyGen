using GalaxyGenCore.Resources;
using GCEngine.Framework;
using System;

namespace GCEngine.Model
{
    public class MarketOrder
    {
        public MarketOrder() 
        {
            MarketOrderId = IdUtils.GetId();
        }

        public Int64 MarketOrderId { get; set; }
        public bool Buy { get; set; }
        public ResourceTypeEnum Type {get; set;}
        public Int64 Quantity { get; set; }
        public Int64 LimitPrice { get; set; }
        public Int64 EntryTick { get; set; }
        public Int64 EventTick { get; set; }

        public Account OwnerAccount { get; set; }      
    }
}