using System.Collections.Generic;

namespace GalaxyGenEngine.Model
{
    public interface IAgentLocation 
    {
        ICollection<Agent> Agents { get; set; }
        TypeEnum GalType { get; set; }
    }    
}