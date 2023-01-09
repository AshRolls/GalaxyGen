using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GCEngine.Engine.Messages
{
    public class MessagePlanetRequestResources : Message
    {
        public MessagePlanetRequestResources(List<ResourceQuantity> resourcesRequested, long ownerId, long storeId, long tickSent)
        {
            ResourcesRequested = resourcesRequested;
            OwnerId = ownerId;
            StoreId = storeId;
            TickSent = tickSent;
        }

        public List<ResourceQuantity> ResourcesRequested { get; private set; }
        public long OwnerId { get; private set; }
        public long StoreId { get; private set; }
    }
}
