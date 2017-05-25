using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Messages
{
    public enum MarketCommandEnum
    {
        GetBuyOrders,               
        PlaceBuyOrder,
        FulfilBuyOrder,
        GetSellOrders,
        PlaceSellOrder,
        FulfilSellOrder
    }

    public class MessageMarketCommand : Message
    {
        public MessageMarketCommand(IMessageMarketCommandData cmd, Int64 tickSent, Int64 planetId)
        {
            Command = cmd;
            TickSent = tickSent;
            PlanetId = planetId;
        }

        public IMessageMarketCommandData Command { get; private set; }
        public Int64 PlanetId { get; private set; }
    }
}
