using GalaxyGenEngine.Framework;
using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{
    public class Account
    { 
        public Account()
        {
            AccountId = IdUtils.GetId();
            Balances = new();
        }

        public UInt64 AccountId { get; set; }

        public IAccountOwner Owner { get; set; }        

        public Dictionary<ulong,long> Balances { get; private set; }
    }
}
