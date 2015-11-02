using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public class Market : IMarket
    {
        [Key]
        public Int64 Id { get; set; }

        [Required]
        public ICollection<IMarketBuyOrder> BuyOrders { get; set; }


    }
}
