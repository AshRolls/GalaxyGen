﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Model
{
    public interface IAgent
    {
        Int64 Id { get; set; }
        String Name { get; set; }

    }
}
