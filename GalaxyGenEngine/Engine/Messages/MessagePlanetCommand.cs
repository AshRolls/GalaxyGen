using GalaxyGenCore.Resources;
using GalaxyGenEngine.Model;
using GalaxyGenEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Messages
{
    public enum PlanetCommandEnum
    {
        RequestLoadShip,
        RequestUnloadShip
    }

    public record MessagePlanetCommand(IMessagePlanetCommandData Command, UInt64 TickSent, UInt64 PlanetScId) : Message(TickSent);

    public record MessagePlanetRequestResources(List<ResourceQuantity> ResourcesRequested, ulong AgentId, ulong StoreId, ulong TickSent) : Message(TickSent);
    public record MessagePlanetRequestShipResources(PlanetCommandEnum CommandType, List<ResourceQuantity> ResourcesRequested, ulong AgentId, ulong ShipId) : IMessagePlanetCommandData;   
}
