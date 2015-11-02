using GalaxyGen.Engine;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalaxyGen.Model
{
    public class MarketBuyOrder : IMarketBuyOrder
    {
        [Key]
        public Int64 Id { get; set; }

        public ResourceTypeEnum Type {get; set;}
        public Int64 Quantity { get; set; }

        [ForeignKey("AgentId")]
        public IAgent Owner { get; set; }        

    }
}