using GalaxyGenCore.Resources;
using System.Collections.Generic;
using System.Data;

namespace GCEngine.Engine.Messages
{
    public class MessagePlanetRequestLoadShipResources : IMessagePlanetCommandData
    {
        public MessagePlanetRequestLoadShipResources(PlanetCommandEnum type, List<ResourceQuantity> resourcesRequested, long ownerId, long shipId)
        {
            CommandType = type;
            ResourcesRequested = resourcesRequested;
            OwnerId = ownerId;
            ShipId = shipId;
        }

        public PlanetCommandEnum CommandType { get; set; }
        public List<ResourceQuantity> ResourcesRequested { get; private set; }
        public long OwnerId { get; private set; }
        public long ShipId { get; private set; }
    }
}
