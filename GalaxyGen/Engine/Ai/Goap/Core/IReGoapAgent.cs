using GalaxyGen.Engine.Controllers;
using System.Collections.Generic;

namespace GalaxyGen.Engine.Goap.Core
{
    public interface IReGoapAgent<T, W>
    {
        List<IReGoapGoal<T, W>> GetGoalsSet();
        List<IReGoapAction<T, W>> GetActionsSet();
        IReGoapMemory<T, W> GetMemory();

        IAgentActions ActionProvider { get; } // this is the class that will perform actions from the goap
        IAgentControllerState StateProvider { get; }
    }
}
