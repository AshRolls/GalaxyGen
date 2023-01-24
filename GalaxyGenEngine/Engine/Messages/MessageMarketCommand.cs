using GalaxyGenCore.Resources;
using GalaxyGenEngine.Model;
using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum MarketCommandEnum
    {
        PlaceSellOrderLowest,
        PlaceSellOrderSpecific,
        SellToHighestBuy,
        BuyFromLowestSell,
        PlaceBuyOrderHighest,
        PlaceBuyOrderSpecific,
        GetBuyOrders,                               
        GetSellOrders,
        GetMarketSnapshot
    }
    public record MessageMarketEmpty(MarketCommandEnum CommandType) : IMessageMarketCommandData;
    public record MessageMarketSpecificPrice(MarketCommandEnum CommandType, ResourceTypeEnum ResourceType, long Quantity, long LimitPrice) : IMessageMarketCommandData;
    public record MessageMarketGeneral(MarketCommandEnum CommandType, ResourceTypeEnum ResourceType, long Quantity) : IMessageMarketCommandData;

    public class MessageMarketCommand : Message
    {
        public MessageMarketCommand(IMessageMarketCommandData cmd, UInt64 agentId, UInt64 tickSent, UInt64 planetScId)
        {
            Command = cmd;
            TickSent = tickSent;
            AgentId = agentId;
            PlanetScId = planetScId;
        }

        public IMessageMarketCommandData Command { get; private set; }
        public UInt64 AgentId { get; private set; }
        public UInt64 PlanetScId { get; private set; }

    }
}
