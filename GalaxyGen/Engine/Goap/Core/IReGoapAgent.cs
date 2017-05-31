using GalaxyGen.Engine.Controllers;
using System.Collections.Generic;

namespace GalaxyGen.Engine.Goap.Core
{
    public interface IReGoapAgent<T, W> : IAgentActions
    {
        IReGoapMemory<T, W> GetMemory();
        IReGoapGoal<T, W> GetCurrentGoal();
        // called from a goal when the goal is available
        void WarnPossibleGoal(IReGoapGoal<T, W> goal);
        //bool IsActive();
        Queue<ReGoapActionState<T, W>> GetStartingPlan();
        W GetPlanValue(T key);
        void SetPlanValue(T key, W value);
        bool HasPlanValue(T target);
        // THREAD SAFE
        List<IReGoapGoal<T, W>> GetGoalsSet();
        List<IReGoapAction<T, W>> GetActionsSet();
        ReGoapState<T, W> InstantiateNewState();        
    }
}
