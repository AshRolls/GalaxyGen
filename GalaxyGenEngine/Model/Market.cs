using System;
using System.Collections.Generic;

namespace GCEngine.Model
{
    public class Market
    { 
        public Int64 MarketId { get; set; }

        public ICollection<MarketBuyOrder> BuyOrders { get; set; }

    }
}
