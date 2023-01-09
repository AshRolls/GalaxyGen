using GalaxyGenCore.Resources;
using GCEngine.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace GCEngine.ViewModel
{
    public class MarketViewModel : IMarketViewModel
    {
        private Dictionary<ResourceTypeEnum, Book> _books;
        
        public MarketViewModel()
        {
            
        }

        private Market model_Var;
        public Market Model
        {
            get { return model_Var; }
            set
            {

                model_Var = value;
                updateFromModel();

                OnPropertyChanged("Market");
            }
        }

        private void updateFromModel()
        {
            foreach (MarketOrder mo in model_Var.Orders.Values)
            {
                Book b;
                if (!_books.TryGetValue(mo.Type, out b)) b = new Book(mo.Type);

                TreeOrder to = new TreeOrder(mo.MarketOrderId, true, mo.Quantity, mo.LimitPrice, mo.EntryTick);
                TreeLimit tl = null;
                long highestBuyPrice = long.MinValue;
                long lowestSellPrice = long.MaxValue;
                if ((mo.Buy && !b.BuyOrders.TryGetValue(mo.LimitPrice, out tl)) || (!mo.Buy && !b.SellOrders.TryGetValue(mo.LimitPrice, out tl))) 
                {
                    tl = new TreeLimit(mo.LimitPrice);
                    tl.head = to;
                    tl.tail = to;
                    if (mo.Buy && mo.LimitPrice > highestBuyPrice)
                    {
                        highestBuyPrice = mo.LimitPrice;
                        b.HighestBuy = tl;
                    }
                    else if (!mo.Buy && mo.LimitPrice < lowestSellPrice)
                    {
                        lowestSellPrice = mo.LimitPrice;
                        b.LowestSell = tl;
                    }
                    if (mo.Buy) b.BuyOrders.Add(mo.LimitPrice, tl);
                    else b.SellOrders.Add(mo.LimitPrice, tl);                    
                }                
                                
                if (tl.tail != to) to.prev = tl.tail;
                tl.tail.next = to;
                tl.tail = to;
                tl.size++;
                tl.totalVolume += mo.Quantity;                                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
    
    public class TreeOrder
    {
        public long id;
        public bool buy;
        public long qty;
        public long limitPrice;
        public long entryTime;
        public long eventTime;
        public TreeOrder prev;
        public TreeOrder next;
        public TreeLimit parentLimit;

        public TreeOrder(long id, bool buy, long qty, long limitPrice, long entryTime)
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
