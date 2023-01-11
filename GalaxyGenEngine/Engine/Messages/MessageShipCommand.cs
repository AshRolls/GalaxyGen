﻿using GalaxyGenEngine.Model;
using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum ShipCommandEnum
    {
        SetXY,
        Undock,
        Dock,
        SetDestination,
        SetAutopilot
    }

    public class MessageShipCommand : Message
    {
        public MessageShipCommand(IMessageShipCommandData cmd, UInt64 tickSent, UInt64 shipId)
        {
            Command = cmd;
            TickSent = tickSent;
            ShipId = shipId;
        }

        public IMessageShipCommandData Command { get; private set; }        
        public UInt64 ShipId { get; private set; }
    }
}
