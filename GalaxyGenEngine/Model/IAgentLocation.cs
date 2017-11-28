using System.Collections.Generic;

namespace GCEngine.Model
{
    public interface IAgentLocation 
    {
        ICollection<Agent> Agents { get; set; }
        TypeEnum GalType { get; set; }
    }    
}