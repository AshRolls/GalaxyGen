using System;
using System.Collections;

/**
 * Collect the world data for this Agent that will be
 * used for GOAP planning.
 */
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    /**
     * Any agent that wants to use GOAP must implement
     * this interface. It provides information to the GOAP
     * planner so it can plan what actions to use.
     * 
     * It also provides an interface for the planner to give 
     * feedback to the Agent and report success/failure.
     */
    public interface IGoap
    {
        /**
         * The starting state of the Agent and the world.
         * Supply what states are needed for actions to run.
         */
        GoapStateBit GetWorldState(GoapPlanner planner);

        /**
         * Give the planner a new goal so it can figure out 
         * the actions needed to fulfill it.
         */
        (GoapStateBit, GoapStateBit) CreateGoalState(GoapPlanner planner);

        /**
         * No sequence of actions could be found for the supplied goal.
         * You will need to try another goal
         */
        void PlanFailed(GoapStateBit failedGoal);

        /**
         * A plan was found for the supplied goal.
         * These are the actions the Agent will perform, in order.
         */
        void PlanFound(GoapStateBit goal, Queue<GoapAction> actions, (int iterations, long ms) stats);

        /**
         * All actions are complete and the goal was reached. Hooray!
         */
        void ActionsFinished();

        /**
         * One of the actions caused the plan to abort.
         * That action is returned.
         */
        void PlanAborted(GoapAction aborter);

        /**
         * Called during Update. Move the agent towards the target in order
         * for the next action to be able to perform.
         * Return true if the Agent is at the target and the next action can perform.
         * False if it is not there yet.
         */
        bool MoveAgent(GoapAction nextAction);

        List<GoapAction> GetActions();     
    }
}
