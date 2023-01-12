using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum EngineRunCommand
    {
        RunMax,
        RunThrottled,
        RunPulse,
        SingleTick,
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
