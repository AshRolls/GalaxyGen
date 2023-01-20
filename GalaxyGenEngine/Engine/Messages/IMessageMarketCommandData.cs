using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public interface IMessageMarketCommandData
    {
        MarketCommandEnum CommandType { get; }
    }
}
