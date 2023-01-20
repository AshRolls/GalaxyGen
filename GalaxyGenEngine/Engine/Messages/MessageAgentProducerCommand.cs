using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    //public record MessageAgentProducerCommand : IMessageAgentCommandData
    //{
    //    public MessageAgentProducerCommand(AgentCommandEnum type, List<ResourceQuantity> resQs, ulong producerId, ulong planetScId)
    //    {
    //        CommandType = type;            
    //        ProducerId = producerId;
    //        PlanetScId = planetScId;
    //        ResQs= resQs;
    //    }

    //    public AgentCommandEnum CommandType { get; private set; }
    //    public ulong ProducerId { get; private set;}
    //    public ulong PlanetScId { get; private set;}
    //    public List<ResourceQuantity> ResQs { get; private set; }
    //}

    public record MessageAgentProducerCommand(AgentCommandEnum CommandType, List<ResourceQuantity> ResQs, ulong ProducerId, ulong PlanetScId) : IMessageAgentCommandData;
}
