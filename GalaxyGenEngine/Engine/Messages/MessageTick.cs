using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public class MessageTick
    {
        public MessageTick(UInt64 tick)
        {
            Tick = tick;
        }

        public UInt64 Tick { get; private set; }
    }
}
