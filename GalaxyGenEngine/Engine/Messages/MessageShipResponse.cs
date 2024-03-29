﻿using GalaxyGenEngine.Model;
using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessageShipResponse : Message
    {
        public MessageShipResponse(bool response, MessageShipCommand sentCommand, UInt64 tickSent)
        {
            Response = response;
            SentCommand = sentCommand;
            TickSent = tickSent;
        }
        
        public Boolean Response { get; private set; }
        public MessageShipCommand SentCommand { get; private set; }
    }
}
