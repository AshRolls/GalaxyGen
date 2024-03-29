﻿using GalaxyGenCore.Resources;
using System.Collections.Generic;
using System.Data;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessagePlanetRequestShipResources : IMessagePlanetCommandData
    {
        public MessagePlanetRequestShipResources(PlanetCommandEnum type, List<ResourceQuantity> resourcesRequested, ulong agentId, ulong shipId)
        {
            CommandType = type;
            ResourcesRequested = resourcesRequested;
            AgentId = agentId;
            ShipId = shipId;
        }

        public PlanetCommandEnum CommandType { get; set; }
        public List<ResourceQuantity> ResourcesRequested { get; private set; }
        public ulong AgentId { get; private set; }
        public ulong ShipId { get; private set; }
    }
}
