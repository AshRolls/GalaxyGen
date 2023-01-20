using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessageMarketGeneral : IMessageMarketCommandData
    {
        public MessageMarketGeneral(MarketCommandEnum type, ResourceTypeEnum res, long qty)
        {
            CommandType = type;
            ResourceType = res;
            Quantity = qty;
        }

        public MarketCommandEnum CommandType { get; private set; }
        public ResourceTypeEnum ResourceType { get; private set; }
        public long Quantity { get; private set; }
    }
}
