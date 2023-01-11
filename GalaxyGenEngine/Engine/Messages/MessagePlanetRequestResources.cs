using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessagePlanetRequestResources : Message
    {
        public MessagePlanetRequestResources(List<ResourceQuantity> resourcesRequested, ulong ownerId, ulong storeId, ulong tickSent)
        {
            ResourcesRequested = resourcesRequested;
            OwnerId = ownerId;
            StoreId = storeId;
            TickSent = tickSent;
        }

        public List<ResourceQuantity> ResourcesRequested { get; private set; }
        public ulong OwnerId { get; private set; }
        public ulong StoreId { get; private set; }
    }
}
