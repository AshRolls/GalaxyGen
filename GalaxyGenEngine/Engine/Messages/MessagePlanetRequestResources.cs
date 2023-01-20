using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessagePlanetRequestResources : Message
    {
        public MessagePlanetRequestResources(List<ResourceQuantity> resourcesRequested, ulong agentId, ulong storeId, ulong tickSent)
        {
            ResourcesRequested = resourcesRequested;
            AgentId = agentId;
            StoreId = storeId;
            TickSent = tickSent;
        }

        public List<ResourceQuantity> ResourcesRequested { get; private set; }
        public ulong AgentId { get; private set; }
        public ulong StoreId { get; private set; }
    }
}
