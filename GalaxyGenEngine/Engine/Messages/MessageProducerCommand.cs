using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum ProducerCommand
    {
        Run,
        Stop
    }

    public class MessageProducerCommand
    {
        public MessageProducerCommand(ProducerCommand prodCmd)
        {
            ProducerCommand = prodCmd;
        }

        public ProducerCommand ProducerCommand { get; private set; }   
        public int ProduceN { get; private set; }     
    }
}
