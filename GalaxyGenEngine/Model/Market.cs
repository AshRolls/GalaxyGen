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

        public ulong MarketId { get; private set; }

        public ulong CurrencyId { get; set; }

        public Dictionary<ulong, MarketOrder> Orders { get; private set; }
    }
}
