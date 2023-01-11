using GalaxyGenEngine.Framework;
using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{
    public enum AgentStateEnum
    {
        PilotingShip,
        PassengerShip,
        Planetside
    }

    public enum AgentTypeEnum
    {
        Default,
        Trader
    }

    public class Agent : IAccountOwner
    {      
        public Agent()
        {
            Producers = new HashSet<Producer>();
            Stores = new HashSet<Store>();
            ShipsOwned = new HashSet<Ship>();
            AgentId = IdUtils.GetId();
        }

        public UInt64 AgentId { get; set; }

        public String Name { get; set; }
        public AgentTypeEnum Type { get; set; }
        public AgentStateEnum AgentState { get; set; }

        public ICollection<Producer> Producers { get; set; }
        public ICollection<Store> Stores { get; set; }
        public ICollection<Ship> ShipsOwned { get; set; }

        public IAgentLocation Location { get; set; }
        public SolarSystem SolarSystem { get; set; }

        public String Memory { get; set; }

        public Account Account { get; set; }
    }
}
