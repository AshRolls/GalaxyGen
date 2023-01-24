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

    public record MessageMarketCommand(IMessageMarketCommandData Command, ulong AgentId, ulong TickSent, ulong PlanetScId) : Message(TickSent);

    public record MessageMarketEmpty(MarketCommandEnum CommandType) : IMessageMarketCommandData;
    public record MessageMarketSpecificPrice(MarketCommandEnum CommandType, ResourceTypeEnum ResourceType, long Quantity, long LimitPrice) : IMessageMarketCommandData;
    public record MessageMarketGeneral(MarketCommandEnum CommandType, ResourceTypeEnum ResourceType, long Quantity) : IMessageMarketCommandData;        
}
