using GalaxyGenEngine.Framework;
using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{
    public class Currency
    { 
        public Currency()
        {
            CurrencyId = IdUtils.GetId();             
        }

        public UInt64 CurrencyId { get; set; }

        public string Name { get; set; }

    }
}
