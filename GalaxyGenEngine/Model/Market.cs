using System;
using System.Collections.Generic;

namespace GCEngine.Model
{
    public class Market
    {
        public Market() 
        { 
            Orders = new Dictionary<long, MarketOrder>();            
        }

        public Int64 MarketId { get; set; }

        public Dictionary<long, MarketOrder> Orders { get; set; }
    }
}
