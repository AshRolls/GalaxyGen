﻿using GalaxyGenEngine.Model;
using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum PlanetCommandEnum
    {
        RequestLoadShip,
        RequestUnloadShip
    }

    public class MessagePlanetCommand : Message
    {
        public MessagePlanetCommand(IMessagePlanetCommandData cmd, Int64 tickSent, Int64 planetScId)
        {
            Command = cmd;
            TickSent = tickSent;
            PlanetScId = planetScId;
        }

        public IMessagePlanetCommandData Command { get; private set; }        
        public Int64 PlanetScId { get; private set; }
    }
}
