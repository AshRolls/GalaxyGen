using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    /**
     * Plans what actions can be completed in order to fulfill a goal state.
     */
    public class GoapPlanner
    {
        public HashSet<GoapAction> UsableActions { get; private set; }
        public Dictionary<GoapStateResLoc, int> ResLocs { get; private set; }
        public GoapStateResLoc[] ResLocsIdx { get; private set; }
        private int _nextResLocIdx = 0;

        public static readonly int FLAGS_COUNT = Enum.GetNames(typeof(GoapStateBitFlagsEnum)).Length - 1;
        public static readonly int ALLOWED_RES_MAX = 2;
        public static readonly int ALLOWED_DEST_MAX = 3;

        public GoapPlanner() 
        {
            UsableActions = new();
            ResLocs = new();
            ResLocsIdx = new GoapStateResLoc[64];
        }

        public void Reset()
        {
            UsableActions.Clear();
            ResLocs.Clear();
            _nextResLocIdx = 0;
            Array.Clear(ResLocsIdx);
        }

        internal (Queue<GoapAction>, (int, long)) PlanBit(GoapAgent agent, HashSet<GoapAction> availableActions, GoapStateBit worldState, GoapStateBit goalState, GoapStateBit allowedState)
        {
            // reset the actions so we can start fresh with them            
            foreach (GoapAction a in availableActions)
            {
                a.DoReset();
                UsableActions.Add(a);
            }            
            
            GoapNodeBit res = null;
            (bool success, (int iterations, long ms) stats) = aStarGraphBit(worldState, ref res, goalState, allowedState, agent);

            // We didn't get a plan
            if (!success) return (null, (0, 0));

            // get its node and work back through the parents to get actions into the correct order
            Stack<GoapAction> plan = new();
            GoapNodeBit n = res;
            while (n != null)
            {
                if (n.Action != null) plan.Push(n.Action);
                n = n.Parent;
            }

            Queue<GoapAction> queue = new();
            foreach (GoapAction a in plan)
            {
                queue.Enqueue(a);
            }

            // hooray we have a plan!
            return (queue, stats);
        }

        private (bool, (int, long)) aStarGraphBit(GoapStateBit worldState, ref GoapNodeBit cur, GoapStateBit goalState, GoapStateBit allowedState, GoapAgent agent)
        {
            PriorityQueue<GoapNodeBit, float> queue = new();

            worldState.AddAllowedResources(allowedState);
            worldState.AddAllowedDestinations(allowedState);
            GoapNodeBit startNode = new(null, worldState, null, 0, goalState, this);            
            queue.Enqueue(startNode, 0);

            Dictionary<GoapStateBit, float> visited = new();                                    
                        
            const int MAX_NODES = 10000;
            const float MAX_COST = 10;            
            int iterations = 0;
            int visitedHits = 0;
            float cost = 0;
            bool found = false;
            cur = null;
            Stopwatch sw = Stopwatch.StartNew();
            while (queue.Count > 0 && iterations < MAX_NODES && cost < MAX_COST)
            {
                cur = queue.Dequeue();
                iterations++;
                cost = cur.Cost;

                // check goal
                if (cur.State.InStateBit(goalState, ResLocs.Count))
                {
                    found = true;
                    break;
                }

                //check if we have explored this state before
                // TODO this doesn't take into account agent position as position is not in state
                if (visited.TryGetValue(cur.State, out float existingCost))
                {
                    if (cur.Cost >= existingCost)
                    {
                        visitedHits++;
                        continue;
                    }
                    else visited[cur.State] = existingCost;
                }
                else visited.Add(cur.State, cur.Cost);

                // expand current nodes
                foreach (GoapNodeBit node in cur.Expand(agent))
                {
                    queue.Enqueue(node, node.Cost);
                }
            }
            sw.Stop();

            return (found, (iterations, sw.ElapsedMilliseconds));
        }

        public int AddResourceLocation(GoapStateResLoc resLoc)
        {
            ResLocs.Add(resLoc, _nextResLocIdx);
            ResLocsIdx[_nextResLocIdx] = resLoc;
            return _nextResLocIdx++;
        }

        public bool TryAddResourceLocation(GoapStateResLoc resLoc, out int idx)
        {
            if (!ResLocs.ContainsKey(resLoc))
            {
                ResLocs.Add(resLoc, _nextResLocIdx);
                ResLocsIdx[_nextResLocIdx] = resLoc;
                idx = _nextResLocIdx++;
                return true;
            }
            idx = 0;
            return false;
        }

        public int GetResourceLocationIdx(GoapStateResLoc resLoc)
        {
            return ResLocs[resLoc];
        }

        public bool TryGetResourceLocationIdx(GoapStateResLoc resLoc, out int idx)
        {            
            if (ResLocs.TryGetValue(resLoc, out int resLocIdx))
            {
                idx = resLocIdx;
                return true;
            }
            idx = 0; 
            return false;
        }

        ///**
        // * Plan what sequence of actions can fulfill the goal.
        // * Returns null if a plan could not be found, or a list of the actions
        // * that must be performed, in order, to fulfill the goal.
        // */
        //public (Queue<GoapAction>,(int,long)) Plan(GoapAgent agent, HashSet<GoapAction> availableActions, GoapState worldState, GoapState goal)
        //{
        //    // reset the actions so we can start fresh with them
        //    foreach (GoapAction a in availableActions)
        //    {
        //        a.DoReset();
        //    }

        //    UsableActions = new HashSet<GoapAction>();
        //    foreach (GoapAction a in availableActions)
        //    {
        //        UsableActions.Add(a);
        //    }

        //    GoapState startingState = new(worldState);
        //    //startingState.AddFromState(goal);
        //    //StartingWorldState = startingState;

        //    GoapState goalGS = new(goal);
        //    GoapNode startNode = new(null, startingState, null, 0, goal, this);
        //    GoapNode res = null;
        //    (bool success, (int iterations, long ms) stats) = aStarGraph(startNode, ref res, goalGS, agent);
        //    //bool success = buildGraph(startNode, leaves, UsableActions, goalGS);
        //    //bool success = aStar(worldState, leaves, goalGS, resourceGoal, agent);

        //    // We didn't get a plan
        //    if (!success) return (null,(0,0));
        
        //    // get its node and work back through the parents to get actions into the correct order
        //    Stack<GoapAction> plan = new();
        //    GoapNode n = res;
        //    while (n != null)
        //    {
        //        if (n.Action != null) plan.Push(n.Action);                
        //        n = n.Parent;
        //    }

        //    Queue<GoapAction> queue = new();
        //    foreach (GoapAction a in plan)
        //    {
        //        queue.Enqueue(a);
        //    }

        //    // hooray we have a plan!
        //    return (queue, stats);
        //}

        //private static (bool,(int,long)) aStarGraph(GoapNode startNode, ref GoapNode cur, GoapState goal, GoapAgent agent)
        //{
        //    PriorityQueue<GoapNode, float> queue = new();
        //    Dictionary<GoapState, float> visited = new();

        //    queue.Enqueue(startNode, 0);

        //    const int MAX_NODES = 10000;
        //    const float MAX_COST = 15;
        //    int iterations = 0;
        //    int visitedHits = 0;
        //    float cost = 0;
        //    bool found = false;
        //    cur = null;
        //    Stopwatch sw = Stopwatch.StartNew();
        //    while (queue.Count > 0 && iterations < MAX_NODES && cost < MAX_COST)
        //    {
        //        cur = queue.Dequeue();
        //        iterations++;
        //        cost = cur.Cost;

        //        // check goal
        //        if (InState(goal, cur.State))
        //        {
        //            found = true;
        //            break;
        //        }

        //        // check if we have explored this state before
        //        if (visited.TryGetValue(cur.State, out float existingCost))
        //        {
        //            if (cur.Cost >= existingCost)
        //            {
        //                visitedHits++;
        //                continue;
        //            }
        //            else visited[cur.State] = existingCost;
        //        }
        //        else visited.Add(cur.State, cur.Cost);

        //        // expand current nodes
        //        foreach (GoapNode node in cur.Expand(agent))
        //        {
        //            queue.Enqueue(node, node.Cost);
        //        }
        //    }
        //    sw.Stop();

        //    return (found,(iterations, sw.ElapsedMilliseconds));
        //}

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

    }
}


