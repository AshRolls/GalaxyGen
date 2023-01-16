using GalaxyGenEngine.Engine.Messages;
using GalaxyGenEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using GalaxyGenCore.Resources;
using C5;
using Akka.Util.Internal;

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
            _books = new();
            // add a book for every resource type
            foreach (ResourceTypeEnum resT in Enum.GetValues(typeof(ResourceTypeEnum)).Cast<ResourceTypeEnum>().Skip(1))
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
            //if ((to.buy && !b.BuyOrders.TryGetValue(to.limitPrice, out tl)) || (!to.buy && !b.SellOrders.TryGetValue(to.limitPrice, out tl)))
            if ((to.buy && !b.BuyOrders.Contains(to.limitPrice)) || (!to.buy && !b.SellOrders.Contains(to.limitPrice)))
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
            else
            {
                if (to.buy) tl = b.BuyOrders[to.limitPrice];
                else tl = b.SellOrders[to.limitPrice]; 
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
                case MarketCommandEnum.PlaceSellOrderLowest:
                    success = placeSellOrderGeneral((MessageMarketGeneral)msg.Command, msg.TickSent, msg.AgentId);
                    break;
                case MarketCommandEnum.PlaceSellOrderSpecific:
                    success = placeSellOrderSpecific((MessageMarketSpecificPrice)msg.Command, msg.TickSent, msg.AgentId);
                    break;
                case MarketCommandEnum.SellToHighestBuy:
                    success = placeBuy((MessageMarketGeneral)msg.Command, msg.TickSent, msg.AgentId);
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

        private bool placeBuy(MessageMarketGeneral msg, ulong tick, ulong agentId)
        {
            Book b = _books[msg.ResourceType];

            if (b.HighestBuy != null) return false;
            throw new NotImplementedException();
            return false;
        }

        private bool placeSellOrderGeneral(MessageMarketGeneral msg, ulong tick, ulong agentId)
        {
            Book b = _books[msg.ResourceType];
            long price = b.LowestSell == null ? 1000 : Math.Max(b.LowestSell.limitPrice - 1,1);
            return placeSellOrder(b, msg.ResourceType, msg.Quantity, agentId, tick, price);
        }

        private bool placeSellOrderSpecific(MessageMarketSpecificPrice msg, ulong tick, ulong agentId)
        {
            Book b = _books[msg.ResourceType];
            return placeSellOrder(b, msg.ResourceType, msg.Quantity, agentId, tick, msg.LimitPrice);
        }

        private bool placeSellOrder(Book b, ResourceTypeEnum resType, long quantity, ulong agentId, ulong tick, long price)
        {
            // check if sell order is lower than highest buy order                
            if (b.HighestBuy != null && b.HighestBuy.limitPrice >= price)
            {
                fillOrder(b, false, resType, quantity, agentId, price, tick);
            }
            return createOrder(b, false, resType, quantity, price, tick, agentId);
        }

        private bool fillOrder(Book b, bool buyOrder, ResourceTypeEnum resType, long quantity, ulong agentId, long price, ulong tick)
        {
            // work out cost by traversing through orders until quantity is filled.
            long totalCost = 0;
            long qty = quantity;
            TreeOrder curOrder = buyOrder ? b.LowestSell.head : b.HighestBuy.head;
            TreeDictionary<long,TreeLimit> orders = buyOrder ? b.SellOrders : b.BuyOrders;

            List<TreeOrder> toFill = new();            
            while (qty >= curOrder.qty)
            {
                qty -= curOrder.qty;
                totalCost += curOrder.qty * curOrder.parentLimit.limitPrice;
                if (curOrder.next != null)
                {
                    toFill.Add(curOrder);
                    curOrder = curOrder.next;
                }
                else if (buyOrder)
                {
                    if (orders.TrySuccessor(curOrder.parentLimit.limitPrice, out C5.KeyValuePair<long, TreeLimit> kvp))
                    {
                        toFill.Add(curOrder);
                        curOrder = kvp.Value.head;
                    }
                    else return false; // not enough depth in book to fill order
                }
                else if (orders.TryPredecessor(curOrder.parentLimit.limitPrice, out C5.KeyValuePair<long, TreeLimit> kvp))
                {
                    toFill.Add(curOrder);
                    curOrder = kvp.Value.head;
                }
                else return false; // not enough depth in book to fill order
            }
            totalCost += qty * curOrder.limitPrice;

            // check with solar system that agent has enough currency to fill these orders.
            if (buyOrder)
            {
                if (!_solarSystemC.CurrencyRequest(_model.CurrencyId, totalCost, agentId, tick)) return false; // not enough funds
            }
            else
            {
                if (!_planetC.ResourceRequest(new ResourceQuantity(resType, quantity), agentId, tick)) return false; // not enough resource 
            }

            // fill orders
            foreach (TreeOrder to in toFill)
            {
                to.parentLimit.totalVolume -= to.qty;
                to.eventTime = tick;
                to.qty = 0;

            }
            curOrder.parentLimit.totalVolume -= qty;
            curOrder.eventTime = tick;
            curOrder.qty -= qty;
            
            if (buyOrder) b.LowestSell = curOrder.parentLimit;
            else b.HighestBuy = curOrder.parentLimit; 
            
            // add new resources to owner
            if (buyOrder) _planetC.ReceiveMarketResource(resType, quantity, agentId);
            else _solarSystemC.AddCurrency(_model.CurrencyId, totalCost, agentId);

            return true;
        }

        private bool createOrder(Book b, bool buyOrder, ResourceTypeEnum resType, long quantity, long limitPrice, ulong tick, ulong agentId)
        {
            if (_planetC.ResourceRequest(new ResourceQuantity(resType, quantity), agentId, tick))
            {
                // create database object and add
                MarketOrder mo = new MarketOrder();
                mo.Buy = buyOrder;
                mo.Quantity = quantity;
                mo.LimitPrice = limitPrice;
                mo.EntryTick = tick;
                mo.OwnerId = agentId;
                _model.Orders.Add(mo.MarketOrderId, mo);

                // create tree object and add
                TreeOrder to = new TreeOrder(mo.MarketOrderId, mo.Buy, mo.Quantity, mo.LimitPrice, mo.EntryTick);
                addTreeOrderToBook(to, b);
                return true;
            }
            return false;
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
        public TreeDictionary<long, TreeLimit> SellOrders;
        public TreeDictionary<long, TreeLimit> BuyOrders;
        public TreeLimit LowestSell;
        public TreeLimit HighestBuy;

        public Book(ResourceTypeEnum type)
        {
            Type = type;
            SellOrders = new TreeDictionary<long, TreeLimit>();
            BuyOrders = new TreeDictionary<long, TreeLimit>();
        }
    }

}
