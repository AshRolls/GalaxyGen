using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap
{
    public class GoapNode
    {        
        private GoapNode parent;
        private GoapState goal;
        private GoapPlanner planner;

        public float Cost { get; private set; }
        public GoapState State { get; private set; }
        public GoapAction Action { get; private set; }
        public float PathCost { get; private set; }
        public float HeuristicCost { get; private set; }
        public HashSet<GoapAction> UsableActions { get; private set; }

        public GoapNode(GoapPlanner planner, GoapNode parent, GoapAction action, GoapState newGoal, HashSet<GoapAction> usableActions)
        {
            this.parent = parent;
            this.Action = action;
            this.planner = planner;
            this.UsableActions = usableActions;

            init(State, newGoal);
        }

        private void init(GoapState state, GoapState newGoal)
        {
            if (this.parent != null)
            {
                this.State = parent.State.Clone();
                this.PathCost = parent.PathCost;
            }

            GoapAction nextAction = parent == null ? null : parent.Action;
            if (this.Action != null)
            {
                this.goal = newGoal + this.Action.Preconditions;
                this.State.AddFromState(this.Action.Effects);

                PathCost += this.Action.GetCost();

                goal.ReplaceWithMissingDifference(this.Action.Effects); // remove current action effects from goal
                goal.ReplaceWithMissingDifference(planner.StartingWorldState); // remove any preconditions already satisfied by world state.
            }
            else
            {
                GoapState diff = new GoapState();
                newGoal.MissingDifference(state, ref diff);
                goal = diff;
            }

            HeuristicCost = goal.Count;
            Cost = PathCost + HeuristicCost;
        }

        public bool IsGoal(GoapNode node)
        {
            return this.HeuristicCost == 0;
        }

        public List<GoapNode> Expand()
        {
            List<GoapNode> expandList = new List<GoapNode>();
            foreach (GoapAction action in this.UsableActions)
            {
                HashSet<GoapAction> newActions = new HashSet<GoapAction>();
                foreach(GoapAction ga in this.UsableActions)
                {
                    if (ga != action) newActions.Add(ga);
                }

                expandList.Add(new GoapNode(this.planner, this, action, goal, newActions));
            }
            return expandList;
        }
    }
}
