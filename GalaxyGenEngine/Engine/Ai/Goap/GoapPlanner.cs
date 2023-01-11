using System;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    /**
     * Plans what actions can be completed in order to fulfill a goal state.
     */
    public class GoapPlanner
    {
        public GoapState StartingWorldState { get; private set; }
        public HashSet<GoapAction> UsableActions { get; private set; }

        /**
         * Plan what sequence of actions can fulfill the goal.
         * Returns null if a plan could not be found, or a list of the actions
         * that must be performed, in order, to fulfill the goal.
         */
        public Queue<GoapAction> Plan(GoapAgent agent, HashSet<GoapAction> availableActions, GoapState worldState, GoapState goal)
        {
            // reset the actions so we can start fresh with them
            foreach (GoapAction a in availableActions)
            {
                a.doReset();
            }

            UsableActions = new HashSet<GoapAction>();
            foreach (GoapAction a in availableActions)
            {
                UsableActions.Add(a);
            }

            GoapState startingState = new GoapState(worldState);
            //startingState.AddFromState(goal);
            //StartingWorldState = startingState;

            GoapState goalGS = new GoapState(goal);
            GoapNode startNode = new GoapNode(null, startingState, null, 0, goal, this);
            GoapNode res = null;
            bool success = aStarGraph(startNode, ref res, goalGS, agent);
            //bool success = buildGraph(startNode, leaves, UsableActions, goalGS);
            //bool success = aStar(worldState, leaves, goalGS, resourceGoal, agent);

            if (!success)
            {
                // oh no, we didn't get a plan
                // Console.WriteLine("NO PLAN");
                return null;
            }

            // get its node and work back through the parents
            List<GoapAction> result = new List<GoapAction>();
            GoapNode n = res;
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

        //private bool aStar(GoapState start, List<GoapNode> leaves, GoapState goal, Dictionary<long, long> resourceGoal, object agent)
        //{
        //    PriorityQueue<GoapNode, float> frontier = new PriorityQueue<GoapNode, float>();
        //    var explored = new Dictionary<GoapState,GoapNode>();
        //    var stateToNode = new Dictionary<GoapState, GoapNode>();

        //    GoapNode root = new GoapNode(this, null, null, start.Clone());
        //    frontier.Enqueue(root, root.Cost);

        //    int i = 0;
        //    int maxIterations = 1000;
        //    while (frontier.Count > 0 && i < maxIterations)
        //    {
        //        i++;

        //        // choose a node we know how to reach
        //        GoapNode node = frontier.Dequeue();

        //        // check if the node is goal, if so add to leaves
        //        if (inState(goal, node.State))
        //        {
        //            leaves.Add(node);
        //        }

        //        // do not repeat ourself
        //        explored.Add(node.State, node);

        //        // where can we get from here that we haven't explored before?
        //        foreach (var child in node.Expand(agent))
        //        {
        //            //First time we see this node?
        //            if (explored.ContainsKey(child.State))
        //                continue;

        //            // If this is a new path, or a shorter path than what we have, keep it.
        //            //GoapNode similiarNode;
        //            //stateToNode.TryGetValue(child.State, out similiarNode);
        //            //if (similiarNode != null)
        //            //{
        //            //    if (similiarNode.Cost > child.Cost) { }
        //            //    //frontier.Remove(similiarNode);
        //            //    else
        //            //        break;
        //            //}

        //            frontier.Enqueue(child, child.Cost);
        //            stateToNode[child.State] = child;
        //        }
        //    }
        //    if (leaves.Count > 0) return true;
        //    // no path found
        //    return false;
        //}

        /**
         * Returns true if at least one solution was found.
         * The possible paths are stored in the leaves list. Each leaf has a
         * 'runningCost' value where the lowest cost will be the best action
         * sequence.
         */
        //private bool buildGraph(GoapNode startNode, List<GoapNode> leaves, HashSet<GoapAction> usableActions, GoapState goal)
        //{
            
        //    foreach (GoapAction action in usableActions)
        //    {
        //        // if the parent state has the conditions for this action's preconditions, we can use it here
        //        if (inState(action.Preconditions, startNode.State) && action.CheckProceduralPrecondition())
        //        {
        //            // apply the action's effects to the parent state
        //            GoapState currentState = populateState(startNode.State, action.Effects);
        //            //Dictionary<Int64, Int64> currentResources = populateResource(parent.resources, action.Resources);

        //            // Console.WriteLine(GoapAgent.PrettyPrint(currentState));
        //            //GoapNode node = new GoapNode(this, parent, parent.Cost + action.GetCost(), parent.Weight + action.GetWeight(), currentState, action);
        //            GoapNode node = new GoapNode(this, startNode, startNode.Cost + action.GetCost(), action, goal);

        //            //if (inState(goal, currentState) && inResources(resourceGoal, currentResources))
        //            if (inState(goal, currentState))
        //            {
        //                // we found a solution!
        //                leaves.Add(node);
        //                currentMaxCost = node.Cost;
        //                foundOne = true;
        //            }
        //            else if (node.Cost < currentMaxCost)
        //            {
        //                // not at a solution yet, so test all the remaining actions and branch out the tree
        //                HashSet<GoapAction> subset = actionSubset(usableActions, action);
        //                bool found = buildGraph(node, leaves, subset, goal);
        //                if (found)
        //                    foundOne = true;
        //            }
        //        }
        //    }

        //    return foundOne;
        //}

        private bool aStarGraph(GoapNode startNode, ref GoapNode cur, GoapState goal, GoapAgent agent)
        {
            PriorityQueue<GoapNode, float> queue = new PriorityQueue<GoapNode, float>();
            Dictionary<GoapState, float> visited = new Dictionary<GoapState, float>();

            queue.Enqueue(startNode, 0);

            const int MAX_NODES = 10000;
            const float MAX_COST = 1000;
            int iterations = 0;
            float cost = 0;
            bool found = false;
            cur = null;
            while (queue.Count > 0 && iterations < MAX_NODES && cost < MAX_COST)
            {
                cur = queue.Dequeue();
                iterations++;
                cost = cur.Cost;

                // check goal
                if (InState(goal, cur.State))
                {
                    found = true;
                    break;
                }

                // check if we have explored this state before
                float existingCost;
                if (visited.TryGetValue(cur.State, out existingCost))
                {
                    if (cur.Cost >= existingCost) continue;
                    else visited[cur.State] = existingCost;
                }
                else visited.Add(cur.State, cur.Cost);

                // expand current nodes
                foreach (GoapNode node in cur.Expand(agent))
                {                                        
                     queue.Enqueue(node, node.Cost);                                        
                }
            }

            return found;
        }

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
        public bool InState(GoapState test, GoapState state)
        {
            var allMatch = true;
            foreach (var t in test.GetValues())
            {
                var match = state.HasKey(t.Key) && state.Get(t.Key).Equals(t.Value);
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
        public GoapState GetNewState(GoapState currentState, GoapState stateChange)
        {
            GoapState state = new GoapState();
            foreach (var s in currentState.GetValues())
            {
                state.Set(s.Key, s.Value);
            }

            foreach (var change in stateChange.GetValues())
            {                
                state.Set(change.Key, change.Value);
            }

            return state;
        }    
    }
}


