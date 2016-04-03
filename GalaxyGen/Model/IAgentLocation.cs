using System.Collections.Generic;

namespace GalaxyGen.Model
{
    public interface IAgentLocation
    {
        ICollection<Agent> Agents { get; set; }
    }
}