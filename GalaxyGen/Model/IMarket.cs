using System;
using System.Collections.Generic;

namespace GalaxyGen.Model
{
    public interface IMarket
    {
        Int64 Id { get; set; }
        ICollection<IMarketBuyOrder> BuyOrders { get; set; }
    }
}