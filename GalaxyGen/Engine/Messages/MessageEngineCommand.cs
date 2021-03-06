﻿using GalaxyGen.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Messages
{
    public enum EngineRunCommand
    {
        RunMax,
        RunPulse,
        Stop
    }

    public class MessageEngineRunCommand
    {
        public MessageEngineRunCommand(EngineRunCommand runCmd)
        {
            RunCommand = runCmd;
        }

        public EngineRunCommand RunCommand { get; private set; }        
    }
}
