using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Messages
{
    public interface IMessageMarketCommandData
    {
        MarketCommandEnum CommandType { get; set; }
        Int64 TargetId { get; set; }
        Int64 Quantity { get; set; }
    }
}
