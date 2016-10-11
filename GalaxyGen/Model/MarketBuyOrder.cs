using GalaxyGen.Engine;
using GalaxyGenCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalaxyGen.Model
{
    public class MarketBuyOrder
    {
        [Key]
        public Int64 MarketBuyOrderId { get; set; }

        public ResourceTypeEnum Type {get; set;}
        public Int64 Quantity { get; set; }

        [ForeignKey("AgentId")]
        public Agent Owner { get; set; }        

    }
}