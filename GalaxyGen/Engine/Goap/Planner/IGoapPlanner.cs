using GalaxyGen.Engine.Goap.Core;
using System;
using System.Collections.Generic;

namespace GalaxyGen.Engine.Goap.Planner
{
    public interface IGoapPlanner<T, W>
    {
        IReGoapGoal<T, W> Plan(IReGoapAgent<T, W> goapAgent, IReGoapGoal<T, W> blacklistGoal, Queue<ReGoapActionState<T, W>> currentPlan, Action<IReGoapGoal<T, W>> callback);
        IReGoapGoal<T, W> GetCurrentGoal();
        IReGoapAgent<T, W> GetCurrentAgent();
        bool IsPlanning();
        ReGoapPlannerSettings GetSettings();
    }
}