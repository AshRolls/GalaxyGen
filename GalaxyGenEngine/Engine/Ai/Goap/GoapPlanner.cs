using System;
using System.Collections.Generic;
using Priority_Queue;

namespace GCEngine.Engine.Ai.Goap
{
    /**
     * Plans what actions can be completed in order to fulfill a goal state.
     */
    public class GoapPlanner
    {
        private static float currentMaxCost;
        public GoapState StartingWorldState { get; private set; }
        public HashSet<GoapAction> UsableActions { get; private set; }

        /**
         * Plan what sequence of actions can fulfill the goal.
         * Returns null if a plan could not be found, or a list of the actions
         * that must be performed, in order, to fulfill the goal.
         */
        public Queue<GoapAction> Plan(object agent, HashSet<GoapAction> availableActions, GoapState worldState, Dictionary<Int64, Int64> resourceState, GoapState goal, Dictionary<Int64, Int64> resourceGoal)
        {
            // reset the actions so we can start fresh with them
            foreach (GoapAction a in availableActions)
            {
                a.doReset();
            }

            // check what actions can run using their checkProceduralPrecondition
            UsableActions = new HashSet<GoapAction>();
            foreach (GoapAction a in availableActions)
            {
                if (a.checkProceduralPrecondition(agent))
                    UsableActions.Add(a);
            }

            // we now have all actions that can run, stored in usableActions

            // build up the tree and record the leaf nodes that provide a solution to the goal.
            List<GoapNode> leaves = new List<GoapNode>();
            // build graph
            currentMaxCost = float.MaxValue;

            GoapState startingState = new GoapState(worldState);
            startingState.AddFromState(goal);
            StartingWorldState = startingState;

            GoapState goalGS = new GoapState(goal);

            //bool success = buildGraph(start, leaves, usableActions, goal, resourceGoal);
            bool success = aStar(worldState, leaves, goalGS, resourceGoal);

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
                //else
                //{
                //    if (leaf.BetterThan(cheapest))
                //        cheapest = leaf;
                //}
            }

            // get its node and work back through the parents
            List<GoapAction> result = new List<GoapAction>();
            GoapNode n = cheapest;
            while (n != null)
            {
                if (n.Action != null)
                {
                    result.Insert(0, n.Action); // insert the action in the front
                }
                n = n.Parent;
            }
            // we now have this action list in correct order

            Queue<GoapAction> queue = new Queue<GoapAction>();
            foreach (GoapAction a in result)
            {
                queue.Enqueue(a);
            }

            // hooray we have a plan!
            return queue;
        }

        private bool aStar(GoapState start, List<GoapNode> leaves, GoapState goal, Dictionary<long, long> resourceGoal)
        {
            var frontier = new SimplePriorityQueue<GoapNode>();
            var explored = new Dictionary<GoapState,GoapNode>();
            var stateToNode = new Dictionary<GoapState, GoapNode>();

            // start at the goal and reverse search back to start
            GoapNode root = new GoapNode(this, null, null, start.Clone());
            frontier.Enqueue(root, root.Cost);

            int i = 0;
            int maxIterations = 1000;
            while (frontier.Count > 0 && i < maxIterations)
            {
                i++;

                // choose a node we know how to reach
                GoapNode node = frontier.Dequeue();

                // check if the node is goal, if so add to leaves
                if (node.IsGoal())
                {
                    leaves.Add(node);
                }

                // do not repeat ourself
                explored.Add(node.State, node);

                // where can we get from here that we haven't explored before?
                foreach (var child in node.Expand())
                {
                    //First time we see this node?
                    if (explored.ContainsKey(child.State))
                        continue;

                    // If this is a new path, or a shorter path than what we have, keep it.
                    GoapNode similiarNode;
                    stateToNode.TryGetValue(child.State, out similiarNode);
                    if (similiarNode != null)
                    {
                        if (similiarNode.Cost > child.Cost)
                            frontier.Remove(similiarNode);
                        else
                            break;
                    }

                    frontier.Enqueue(child, child.Cost);
                    stateToNode[child.State] = child;
                }
            }
            if (leaves.Count > 0) return true;
            // no path found
            return false;
        }

        /**
         * Returns true if at least one solution was found.
         * The possible paths are stored in the leaves list. Each leaf has a
         * 'runningCost' value where the lowest cost will be the best action
         * sequence.
         */
        //private bool buildGraph(GoapNode parent, List<GoapNode> leaves, HashSet<GoapAction> usableActions, Dictionary<string, object> goal, Dictionary<Int64, Int64> resourceGoal)
        //{
        //    bool foundOne = false;

        //    // go through each action available at this node and see if we can use it here
        //    foreach (GoapAction action in usableActions)
        //    {

        //        // if the parent state has the conditions for this action's preconditions, we can use it here
        //        if (inState(action.Preconditions, parent.state))
        //        {
        //            // apply the action's effects to the parent state
        //            Dictionary<string, object> currentState = populateState(parent.state, action.Effects);
        //            Dictionary<Int64, Int64> currentResources = populateResource(parent.resources, action.Resources);

        //            // Console.WriteLine(GoapAgent.PrettyPrint(currentState));
        //            GoapNode node = new GoapNode(parent, parent.cost + action.GetCost(), parent.weight + action.GetWeight(), currentState, currentResources, action);                    

        //            if (inState(goal, currentState) && inResources(resourceGoal, currentResources))
        //            {
        //                // we found a solution!
        //                leaves.Add(node);
        //                currentMaxCost = node.cost;
        //                foundOne = true;
        //            }
        //            else if (node.cost < currentMaxCost)
        //            {
        //                // not at a solution yet, so test all the remaining actions and branch out the tree
        //                HashSet<GoapAction> subset = actionSubset(usableActions, action);
        //                bool found = buildGraph(node, leaves, subset, goal, resourceGoal);
        //                if (found)
        //                    foundOne = true;
        //            }
        //        }
        //    }

        //    return foundOne;
        //}

        /**
         * Create a subset of the actions excluding the removeMe one. Creates a new set.
         */
        private HashSet<GoapAction> actionSubset(HashSet<GoapAction> actions, GoapAction removeMe)
        {
            HashSet<GoapAction> subset = new HashSet<GoapAction>();
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

        private bool inResources(Dictionary<Int64, Int64> resourceGoal, Dictionary<Int64, Int64> currentResources)
        {
            var allMatch = true;
            foreach (var t in resourceGoal)
            {
                var match = currentResources.ContainsKey(t.Key) && currentResources[t.Key] >= t.Value;
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
            Dictionary<string, object> state = new Dictionary<string, object>();
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

        private Dictionary<Int64, Int64> populateResource(Dictionary<Int64, Int64> currentResource, Dictionary<Int64, Int64> resourceChange)
        {
            Dictionary<Int64, Int64> resources = new Dictionary<Int64, Int64>();

            foreach (var res in currentResource)
            {
                resources.Add(res.Key, res.Value);
            }

            foreach (var res in resourceChange)
            {
                if (!resources.ContainsKey(res.Key))
                {
                    resources.Add(res.Key, res.Value);
                }
                else
                {
                    resources[res.Key] += res.Value;
                }
            }

            return resources;
        }

    }
}


