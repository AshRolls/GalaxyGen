using GCEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Messages
{
    public class MessageTick
    {
        public MessageTick(Int64 tick)
        {
            Tick = tick;
        }

        public Int64 Tick { get; private set; }
    }
}
