using Akka.Actor;
using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Model;
using GalaxyGenCore.BluePrints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGenCore.Resources;
using System.Diagnostics;

namespace GalaxyGenEngine.Engine.Controllers
{
    public class MarketController
    {
        private Market _model;
        private PlanetController _planetC;
        private SolarSystemController _solarSystemC;
        private TextOutputController _textOutput;
        private Dictionary<ResourceTypeEnum, Book> _books;

        public MarketController(Market m, PlanetController pc, SolarSystemController ssc, TextOutputController textOutput)
        {
            _model = m;
            _planetC = pc;
            _solarSystemC = ssc;
            _textOutput = textOutput;
            setupMarket();
        }

        private void setupMarket()
        {
            // add a book for every resource type
            foreach (ResourceTypeEnum resT in Enum.GetValues(typeof(ResourceTypeEnum)))
            {
                _books.Add(resT, new Book(resT));
            }

            foreach (MarketOrder mo in _model.Orders.Values)
            {
                Book b = _books[mo.Type];
                TreeOrder to = new TreeOrder(mo.MarketOrderId, true, mo.Quantity, mo.LimitPrice, mo.EntryTick);
                addTreeOrderToBook(to, b);                
            }
        }

        private void addTreeOrderToBook(TreeOrder to, Book b)
        {
            TreeLimit tl = null;
            long highestBuyPrice = b.HighestBuy == null ? long.MinValue : b.HighestBuy.limitPrice;
            long lowestSellPrice = b.LowestSell == null ? long.MaxValue : b.LowestSell.limitPrice;
            if ((to.buy && !b.BuyOrders.TryGetValue(to.limitPrice, out tl)) || (!to.buy && !b.SellOrders.TryGetValue(to.limitPrice, out tl)))
            {
                tl = new TreeLimit(to.limitPrice);
                tl.head = to;
                tl.tail = to;
                if (to.buy && to.limitPrice > highestBuyPrice)
                {                    
                    b.HighestBuy = tl;
                }
                else if (!to.buy && to.limitPrice < lowestSellPrice)
                {                 
                    b.LowestSell = tl;
                }
                if (to.buy) b.BuyOrders.Add(to.limitPrice, tl);
                else b.SellOrders.Add(to.limitPrice, tl);
            }
            if (tl.tail != to) to.prev = tl.tail;
            tl.tail.next = to;
            tl.tail = to;
            tl.size++;
            tl.totalVolume += to.qty;
            to.parentLimit = tl;
        }

        internal void receiveMarketCommand(MessageMarketCommand msg)
        {
            // check the ship *could* execute this command
            bool success;
            switch (msg.Command.CommandType)
            {
                case MarketCommandEnum.PlaceSellOrder:
                    success = PlaceSellOrder((MessageMarketBasic)msg.Command, msg.TickSent, msg.AgentId);
                    break;
                
                default:
                    success = false;
                    throw new Exception("Unknown Market Command");
            }
            if (!success)
            {
                _solarSystemC.SendMessageToAgent(msg.AgentId, new MessageAgentCommand(new MessageAgentFailedCommand(AgentCommandEnum.ShipCommandFailed), msg.TickSent));
            }
        }

        private bool PlaceSellOrder(MessageMarketBasic msg, ulong tick, ulong agentId)
        {
            Book b = _books[msg.ResourceType];
            MarketOrder mo = new MarketOrder();
            mo.Buy = false;
            mo.Quantity = msg.Quantity;
            mo.LimitPrice = msg.LimitPrice;
            mo.EntryTick = tick;
            mo.OwnerId = agentId;
            _model.Orders.Add(mo.MarketOrderId, mo);

            TreeOrder to = new TreeOrder(mo.MarketOrderId, false, mo.Quantity, mo.LimitPrice, mo.EntryTick);
            addTreeOrderToBook(to, b);

            return true;
        }
    }

    public class TreeOrder
    {
        public ulong id;
        public bool buy;
        public long qty;
        public long limitPrice;
        public ulong entryTime;
        public ulong eventTime;
        public TreeOrder prev;
        public TreeOrder next;
        public TreeLimit parentLimit;

        public TreeOrder(ulong id, bool buy, long qty, long limitPrice, ulong entryTime)
        {
            this.id = id;
            this.buy = buy;
            this.qty = qty;
            this.limitPrice = limitPrice;
            this.entryTime = entryTime;
        }
    }

    public class TreeLimit
    {
        public long limitPrice;
        public int size;
        public long totalVolume;
        public TreeOrder head;
        public TreeOrder tail;

        public TreeLimit(long limitPrice)
        {
            this.limitPrice = limitPrice;
            size = 0;
            totalVolume = 0;
        }
    }

    public class Book
    {
        public ResourceTypeEnum Type;
        public SortedDictionary<long, TreeLimit> SellOrders;
        public SortedDictionary<long, TreeLimit> BuyOrders;
        public TreeLimit LowestSell;
        public TreeLimit HighestBuy;

        public Book(ResourceTypeEnum type)
        {
            Type = type;
            SellOrders = new SortedDictionary<long, TreeLimit>();
            BuyOrders = new SortedDictionary<long, TreeLimit>();
        }
    }

}
