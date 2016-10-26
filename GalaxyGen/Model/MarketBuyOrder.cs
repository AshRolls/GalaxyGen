using GalaxyGen.Engine;
using GalaxyGenCore;
using GalaxyGenCore.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalaxyGen.Model
{
    public class MarketBuyOrder
    {
        public Int64 MarketBuyOrderId { get; set; }

        public ResourceTypeEnum Type {get; set;}
        public Int64 Quantity { get; set; }
        public Int64 Price { get; set; }

        public Account OwnerAccount { get; set; }      
    }
}