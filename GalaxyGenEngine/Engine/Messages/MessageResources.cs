using GalaxyGenEngine.Model;
using GalaxyGenEngine.ViewModel;
using GalaxyGenCore;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessageResources : Message
    {
        public MessageResources(List<ResourceQuantity> resources, UInt64 tickSent)
        {
            Resources = resources;
            TickSent = tickSent;
        }

        public List<ResourceQuantity> Resources { get; private set; }
    }
}
