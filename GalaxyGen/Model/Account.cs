using GalaxyGen.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
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
