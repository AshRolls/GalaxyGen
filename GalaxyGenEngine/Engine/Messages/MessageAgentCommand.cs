using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum AgentCommandEnum
    {
        ShipCommandFailed,
        PlanetCommandFailed,
        MarketCommandFailed,
        ProducerStartedProducing,
        ProducerStoppedProducing,
        MarketSnapshot
    }
    public record MessageAgentFailedCommand(AgentCommandEnum CommandType) : IMessageAgentCommandData;
    public record MessageAgentProducerCommand(AgentCommandEnum CommandType, List<ResourceQuantity> ResQs, ulong ProducerId, ulong PlanetScId) : IMessageAgentCommandData;
    public record MessageAgentMarketSnapshot(AgentCommandEnum CommandType, List<(ResourceTypeEnum, long)> spotPrices, ulong PlanetScId) : IMessageAgentCommandData;

    public class MessageAgentCommand : Message
    {
        public MessageAgentCommand(IMessageAgentCommandData cmd, UInt64 tickSent)
        {
            Command = cmd;
            TickSent = tickSent;
        }

        public IMessageAgentCommandData Command { get; private set; }        
    }
}
