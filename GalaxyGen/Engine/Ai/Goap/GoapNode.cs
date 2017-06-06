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
        private float cost;
        private GoapState state;
        private GoapState goal;
        private GoapPlanner planner;

        public GoapAction Action { get; private set; }
        public float PathCost { get; private set; }
        public float HeuristicCost { get; private set; }

        public GoapNode(GoapPlanner planner, GoapNode parent, GoapAction action, GoapState newGoal)
        {
            this.parent = parent;
            this.Action = action;

            init(state, newGoal);
        }

        private void init(GoapState state, GoapState newGoal)
        {
            if (this.parent != null)
            {
                this.state = parent.state.Clone();
                this.PathCost = parent.PathCost;
            }

            GoapAction nextAction = parent == null ? null : parent.Action;
            if (this.Action != null)
            {
                this.goal = newGoal + this.Action.Preconditions;
                this.state.AddFromState(this.Action.Effects);

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
            cost = PathCost + HeuristicCost;
        }

        public bool IsGoal(GoapNode node)
        {
            return this.HeuristicCost == 0;
        }

        internal float GetCost()
        {
            return cost;
        }
    }
}
