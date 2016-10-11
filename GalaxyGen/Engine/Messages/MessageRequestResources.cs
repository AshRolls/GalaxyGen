using GalaxyGen.Model;
using GalaxyGen.ViewModel;
using GalaxyGenCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine
{
    public class MessageRequestResources : Message
    {
        public MessageRequestResources(List<ResourceQuantity> resourcesRequested, Agent owner, Int64 tickSent)
        {
            ResourcesRequested = resourcesRequested;
            Owner = owner;
            TickSent = tickSent;
        }

        public List<ResourceQuantity> ResourcesRequested { get; private set; }
        public Agent Owner { get; private set; }
    }
}
