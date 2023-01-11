using GalaxyGenEngine.Framework;
using System;

namespace GalaxyGenEngine.Model
{
    public class Account
    { 
        public Account()
        {
            AccountId = IdUtils.GetId();
        }

        public UInt64 AccountId { get; set; }

        public IAccountOwner Owner { get; set; }        

        public Int64 Balance { get; set; }
    }
}
