using System;
using System.Collections.Generic;

namespace GalaxyGen.Engine.Goap.Core
{
    public interface IReGoapAction<T, W>
    {
        // this should return current's action calculated parameter, will be added to the run method
        // userful for dynamic actions, for example a GoTo action can save some informations (wanted position)
        // while being chosen from the planner, we save this information and give it back when we run the method
        IReGoapActionSettings<T, W> GetSettings(IReGoapAgent<T, W> goapAgent, ReGoapState<T, W> goalState);
        bool Perform(IReGoapActionSettings<T, W> settings, ReGoapState<T, W> goalState);
        string Name { get; }    
        void PostPlanCalculations(IReGoapAgent<T, W> goapAgent);
        void Precalculations(IReGoapAgent<T, W> goapAgent, ReGoapState<T, W> goalState);
        ReGoapState<T, W> GetPreconditions(ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null);
        ReGoapState<T, W> GetEffects(ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null);
        bool CheckProceduralCondition(IReGoapAgent<T, W> goapAgent, ReGoapState<T, W> goalState, IReGoapAction<T, W> nextAction = null);
        float GetCost(ReGoapState<T, W> goalState, IReGoapAction<T, W> next = null);
        bool RequiresInRange();
        Int64 TargetScId { get; }
        void SetInRange(bool inRange);
        bool IsInRange();
        bool IsDone();

    }

    public interface IReGoapActionSettings<T, W>
    {
    }
}