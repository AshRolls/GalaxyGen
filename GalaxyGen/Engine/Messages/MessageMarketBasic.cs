using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Messages
{
    public class MessageMarketBasic : IMessageMarketCommandData
    {
        public MessageMarketBasic(MarketCommandEnum type, Int64 targetId, Int64 quantity)
        {
            CommandType = type;
        }

        public MarketCommandEnum CommandType { get; set; }
        public Int64 TargetId { get; set; }
        public Int64 Quantity { get; set; }

    }
}
