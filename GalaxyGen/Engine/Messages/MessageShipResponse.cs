using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class MessageShipResponse : Message
    {
        public MessageShipResponse(bool response, MessageShipCommand sentCommand, Int64 tickSent)
        {
            Response = response;
            SentCommand = sentCommand;
            TickSent = tickSent;
        }
        
        public Boolean Response { get; private set; }
        public MessageShipCommand SentCommand { get; private set; }
    }
}
