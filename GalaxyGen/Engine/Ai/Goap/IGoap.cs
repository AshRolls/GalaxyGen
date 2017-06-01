using GalaxyGen.Engine.Goap.Core;
using System;
using System.Collections;

/**
 * Collect the world data for this Agent that will be
 * used for GOAP planning.
 */
using System.Collections.Generic;

namespace GalaxyGen.Engine.Ai.Goap
{
    /**
     * Any agent that wants to use GOAP must implement
     * this interface. It provides information to the GOAP
     * planner so it can plan what actions to use.
     * 
     * It also provides an interface for the planner to give 
     * feedback to the Agent and report success/failure.
     */
    public interface IGoap<T, W>
    {
        //Dictionary<Int64, Int64> CreateResourceGoal();

        /**
         * No sequence of actions could be found for the supplied goal.
         * You will need to try another goal
         */
        void PlanFailed(IReGoapGoal<T,W> plan);

        /**
         * A plan was found for the supplied goal.
         * These are the actions the Agent will perform, in order.
         */
        void PlanFound(IReGoapGoal<T,W> plan);

        /**
         * All actions are complete and the goal was reached. Hooray!
         */
        void ActionsFinished();

        /**
         * One of the actions caused the plan to abort.
         * That action is returned.
         */
        void PlanAborted(ReGoapActionState<T, W> aborterAction);

        /**
         * Called during Update. Move the agent towards the target in order
         * for the next action to be able to perform.
         * Return true if the Agent is at the target and the next action can perform.
         * False if it is not there yet.
         */
        bool MoveAgent(ReGoapActionState<T,W> nextAction);

        List<IReGoapGoal<T, W>> GetGoals();
        List<IReGoapAction<T, W>> GetActions();

    }
}
