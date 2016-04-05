using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class MessageShipDockResponse : Message
    {
        public MessageShipDockResponse(bool response, Int64 tickSent)
        {
            Response = response;
            TickSent = tickSent;
        }
        
        public Boolean Response { get; private set; }
    }
}
