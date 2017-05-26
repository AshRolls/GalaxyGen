using System;
using System.Collections.Generic;


namespace GalaxyGen.Engine.Ai.Goap
{
    /**
     * Plans what actions can be completed in order to fulfill a goal state.
     */
    public class GoapPlanner
    {

        /**
         * Plan what sequence of actions can fulfill the goal.
         * Returns null if a plan could not be found, or a list of the actions
         * that must be performed, in order, to fulfill the goal.
         */
        public Queue<GoapAction> Plan(object agent, HashSet<GoapAction> availableActions, Dictionary<string, object> worldState, Dictionary<string, object> goal)
        {
            // reset the actions so we can start fresh with them
            foreach (GoapAction a in availableActions)
            {
                a.doReset();
            }

            // check what actions can run using their checkProceduralPrecondition
            HashSet<GoapAction> usableActions = NodeManager.GetFreeActionSet();
            foreach (GoapAction a in availableActions)
            {
                if (a.checkProceduralPrecondition(agent))
                    usableActions.Add(a);
            }

            // we now have all actions that can run, stored in usableActions

            // build up the tree and record the leaf nodes that provide a solution to the goal.
            List<GoapNode> leaves = new List<GoapNode>();

            // build graph
            GoapNode start = NodeManager.GetFreeNode(null, 0, 0, worldState, null);
            bool success = buildGraph(start, leaves, usableActions, goal);

            if (!success)
            {
                // oh no, we didn't get a plan
                // Console.WriteLine("NO PLAN");
                return null;
            }

            // get the cheapest leaf
            GoapNode cheapest = null;
            foreach (GoapNode leaf in leaves)
            {
                if (cheapest == null)
                    cheapest = leaf;
                else
                {
                    if (leaf.BetterThan(cheapest))
                        cheapest = leaf;
                }
            }

            // get its node and work back through the parents
            List<GoapAction> result = new List<GoapAction>();
            GoapNode n = cheapest;
            while (n != null)
            {
                if (n.action != null)
                {
                    result.Insert(0, n.action); // insert the action in the front
                }
                n = n.parent;
            }

            NodeManager.Release();
            // we now have this action list in correct order

            Queue<GoapAction> queue = new Queue<GoapAction>();
            foreach (GoapAction a in result)
            {
                queue.Enqueue(a);
            }

            // hooray we have a plan!
            return queue;
        }

        /**
         * Returns true if at least one solution was found.
         * The possible paths are stored in the leaves list. Each leaf has a
         * 'runningCost' value where the lowest cost will be the best action
         * sequence.
         */
        private bool buildGraph(GoapNode parent, List<GoapNode> leaves, HashSet<GoapAction> usableActions, Dictionary<string, object> goal)
        {
            bool foundOne = false;

            // go through each action available at this node and see if we can use it here
            foreach (GoapAction action in usableActions)
            {

                // if the parent state has the conditions for this action's preconditions, we can use it here
                if (inState(action.Preconditions, parent.state))
                {

                    // apply the action's effects to the parent state
                    Dictionary<string, object> currentState = populateState(parent.state, action.Effects);
                    // Console.WriteLine(GoapAgent.PrettyPrint(currentState));
                    GoapNode node = NodeManager.GetFreeNode(parent, parent.runningCost + action.GetCost(), parent.weight + action.GetWeight(), currentState, action);

                    //force child.precondition in parent.effects or child.precondition is empty.
                    if (action.Preconditions.Count == 0 && parent.action != null ||
                        parent.action != null && !CondRelation(action.Preconditions, parent.action.Effects))
                        continue;

                    if (inState(goal, currentState))
                    {
                        // we found a solution!
                        leaves.Add(node);
                        foundOne = true;
                    }
                    else
                    {
                        // not at a solution yet, so test all the remaining actions and branch out the tree
                        HashSet<GoapAction> subset = actionSubset(usableActions, action);
                        bool found = buildGraph(node, leaves, subset, goal);
                        if (found)
                            foundOne = true;
                    }
                }
            }

            return foundOne;
        }

        /**
         * Create a subset of the actions excluding the removeMe one. Creates a new set.
         */
        private HashSet<GoapAction> actionSubset(HashSet<GoapAction> actions, GoapAction removeMe)
        {
            HashSet<GoapAction> subset = NodeManager.GetFreeActionSet();
            foreach (GoapAction a in actions)
            {
                if (!a.Equals(removeMe))
                    subset.Add(a);
            }
            return subset;
        }

        /**
         * Check that all items in 'test' are in 'state'. If just one does not match or is not there
         * then this returns false.
         */
        private bool inState(Dictionary<string, object> test, Dictionary<string, object> state)
        {
            var allMatch = true;
            foreach (var t in test)
            {
                var match = state.ContainsKey(t.Key) && state[t.Key].Equals(t.Value);
                if (!match)
                {
                    allMatch = false;
                    break;
                }
            }
            return allMatch;
        }

        //if there is one true relationship
        private bool CondRelation(Dictionary<string, object> preconditions
                                , Dictionary<string, object> effects)
        {
            foreach (var t in preconditions)
            {
                var match = effects.ContainsKey(t.Key) && effects[t.Key].Equals(t.Value);
                if (match)
                    return true;
            }
            return false;
        }

        /**
         * Apply the stateChange to the currentState
         */
        private Dictionary<string, object> populateState(Dictionary<string, object> currentState, Dictionary<string, object> stateChange)
        {
            Dictionary<string, object> state = NodeManager.GetFreeState();
            state.Clear();
            foreach (var s in currentState)
            {
                state.Add(s.Key, s.Value);
            }

            foreach (var change in stateChange)
            {
                // if the key exists in the current state, update the Value
                if (state.ContainsKey(change.Key))
                {
                    state[change.Key] = change.Value;
                }
                else
                {
                    state.Add(change.Key, change.Value);
                }
            }
            return state;
        }

    }
}


