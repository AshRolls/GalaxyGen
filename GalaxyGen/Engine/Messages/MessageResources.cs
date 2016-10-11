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
    public class MessageResources : Message
    {
        public MessageResources(List<ResourceQuantity> resources, Int64 tickSent)
        {
            Resources = resources;
            TickSent = tickSent;
        }

        public List<ResourceQuantity> Resources { get; private set; }
    }
}
