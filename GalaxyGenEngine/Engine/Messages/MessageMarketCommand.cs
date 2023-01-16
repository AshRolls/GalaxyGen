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
        GetBuyOrders,               
        PlaceBuyOrder,
        FulfilBuyOrder,
        GetSellOrders,
        PlaceSellOrder,
        FulfilSellOrder
    }

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
