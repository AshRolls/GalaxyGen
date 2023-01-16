using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessageMarketSpecificPrice : IMessageMarketCommandData
    {
        public MessageMarketSpecificPrice(MarketCommandEnum type, ResourceTypeEnum res, long qty, long limitPrice)
        {
            CommandType = type;
            ResourceType = res;
            Quantity = qty;
            LimitPrice = limitPrice;
        }

        public MarketCommandEnum CommandType { get; private set; }
        public ResourceTypeEnum ResourceType { get; private set; }
        public long Quantity { get; private set; }
        public long LimitPrice { get; private set; }

    }
}
