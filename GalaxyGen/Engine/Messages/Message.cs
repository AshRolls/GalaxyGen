﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Messages
{
    public abstract class Message
    {
        public Int64 TickSent { get; protected set; }
    }
}
