using GCEngine.Framework;
using System;

namespace GCEngine.Model
{
    public class Account
    { 
        public Account()
        {
            AccountId = IdUtils.GetId();
        }

        public Int64 AccountId { get; set; }

        public IAccountOwner Owner { get; set; }        

        public Int64 Balance { get; set; }
    }
}
