using Akka.Actor;
using GCEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine
{
    public class ActorTextOutput : ReceiveActor
    {
        ITextOutputViewModel _textOutputVm;

        public ActorTextOutput(ITextOutputViewModel textOutputVm)
        {
            _textOutputVm = textOutputVm;

            Receive<String>(msg => writeText(msg));
        }

        private void writeText(String msg)
        {
            _textOutputVm.AddLine(msg);
        }

    }    
}
