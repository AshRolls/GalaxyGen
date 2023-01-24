using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum ProducerCommandEnum
    {
        Run,
        Stop
    }

    public record MessageProducerCommand(ProducerCommandEnum Command, int ProduceN);
}
