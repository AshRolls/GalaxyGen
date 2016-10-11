using System;
using System.Collections.Generic;

namespace GalaxyGen.Model
{
    public interface IReadOnlyAgent
    {
        Int64 AgentId { get; }
        AgentStateEnum AgentState { get;  }
        IAgentLocation Location { get;  }
        string Name { get; }
        ICollection<Producer> Producers { get;  }
        ICollection<Ship> ShipsOwned { get;}
        SolarSystem SolarSystem { get;  }
        ICollection<Store> Stores { get; }
        AgentTypeEnum Type { get;  }
    }
}