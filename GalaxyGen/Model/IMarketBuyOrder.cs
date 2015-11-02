using GalaxyGen.Engine;
using System;

namespace GalaxyGen.Model
{
    public interface IMarketBuyOrder
    {
        Int64 Id { get; set; }
        IAgent Owner { get; set; }
        Int64 Quantity { get; set; }
        ResourceTypeEnum Type { get; set; }
    }
}