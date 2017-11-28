using GCEngine.Model;
using GalaxyGenCore;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Messages
{
    public class MessageProducedResources
    {
        public MessageProducedResources(List<ResourceQuantity> resources, Agent owner)
        {
            Resources = resources;
            Owner = owner;
        }

        public List<ResourceQuantity> Resources { get; set; }
        public Agent Owner { get; set; }
    }
}
