using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{
    public class Market
    {
        public Market() 
        { 
            Orders = new Dictionary<ulong, MarketOrder>();            
        }

        public Int64 MarketId { get; set; }

        public Dictionary<ulong, MarketOrder> Orders { get; set; }
    }
}
