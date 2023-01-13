using GalaxyGenEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    public class GoapNode
    {                
        private GoapState goal;
        private GoapPlanner planner;

        public GoapNode Parent { get; private set; }
        public float Cost { get; private set; }
        public GoapState State { get; private set; }
        public GoapAction Action { get; private set; }
        public float PathCost { get; private set; }
        public float HeuristicCost { get; private set; }

        public GoapNode(GoapNode parent, GoapState state, GoapAction action, float pathCost, GoapState newGoal, GoapPlanner planner)
        {
            this.Parent = parent;
            this.Action = action;
            this.State = state;
            this.PathCost = pathCost;
            this.planner = planner;
            
            init(newGoal);
        }

        private void init(GoapState newGoal)
        {
            //if (this.Parent != null)
            //{
            //    this.State = Parent.State.Clone();
            //    this.PathCost = Parent.PathCost;
            //}
            //else
            //{
            //    this.State = planner.StartingWorldState.Clone();                
            //}
            
            if (this.Action != null)
            {
                this.goal = newGoal;
                //this.State.AddFromState(this.Action.Effects);

                if (this.Action.IsSpecific()) PathCost += this.Action.GetCost();

                goal.ReplaceWithMissingDifference(this.Action.Effects); // remove current action effects from goal
                //goal.ReplaceWithMissingDifference(planner.StartingWorldState); // remove any preconditions already satisfied by world state.
            }
            else
            {
                GoapState diff = new GoapState();
                newGoal.MissingDifference(this.State, ref diff);
                goal = diff;
            }

            HeuristicCost = goal.Count;
            Cost = PathCost + HeuristicCost;
        }

        public List<GoapNode> Expand(GoapAgent agent)
        {
            List<GoapNode> expandList = new List<GoapNode>();
            foreach (GoapAction action in planner.UsableActions)
            {
                if (GoapPlanner.InState(action.Preconditions, this.State) && action.CheckProceduralPrecondition(agent))
                {
                    if (action.IsSpecific())
                    {
                        GoapState newState = GoapPlanner.GetNewState(this.State, action.Effects);
                        GoapNode newNode = new GoapNode(this, newState, action, this.PathCost, goal.Clone(), planner);
                        expandList.Add(newNode);
                    }
                    else
                    {
                        foreach(GoapAction sAction in action.GetSpecificActions(agent, this.State, this.goal))
                        {
                            GoapState newState = GoapPlanner.GetNewState(this.State, sAction.Effects);
                            GoapNode newNode = new GoapNode(this, newState, sAction, this.PathCost, goal.Clone(), planner);
                            expandList.Add(newNode);
                        }
                    }
                }
            }
            return expandList;
        }

        //public List<GoapNode> Expand(object agent)
        //{
        //    List<GoapNode> expandList = new List<GoapNode>();
        //    foreach (GoapAction action in planner.UsableActions)
        //    {
        //        if (action == this.Action || !action.checkProceduralPrecondition(agent))    
        //            continue;                

        //        //if (!this.State.HasAnyConflict(action.Effects))
        //        //{
        //            expandList.Add(new GoapNode(this.planner, this, action, goal));
        //        //}
        //    }
        //    return expandList;
        //}

        //private bool checkActionValid(GoapAction ga)
        //{
        //    bool valid = true;
        //    // if the current state contains the action effect, then the effect must equal the state
        //    foreach (KeyValuePair<string, object> kvp in ga.Effects.GetValues())
        //    {
        //        if (this.State.HasKey(kvp.Key))
        //        {
        //            if (!this.State.Get(kvp.Key).Equals(kvp.Value))
        //            {
        //                valid = false;
        //                break;
        //            }

        //        }
        //    }
        //    return valid;
        //}
    }
}
