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

    public record MessageAgentCommand(IMessageAgentCommandData Command, UInt64 TickSent) : Message(TickSent);

    public record MessageAgentFailedCommand(AgentCommandEnum CommandType) : IMessageAgentCommandData;
    public record MessageAgentProducerCommand(AgentCommandEnum CommandType, List<ResourceQuantity> ResQs, ulong ProducerId, ulong PlanetScId) : IMessageAgentCommandData;
    public record MessageAgentMarketSnapshot(AgentCommandEnum CommandType, List<(ResourceTypeEnum, long)> SpotPrices, ulong PlanetScId) : IMessageAgentCommandData;

    
    
}
