using GalaxyGenEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap
{
    public class GoapNodeBit
    {                
        private GoapStateBit _goalState;
        private GoapPlanner _planner;
        private float _heuristicCost;

        public GoapNodeBit Parent { get; private set; }
        public float Cost { get; private set; }
        public GoapStateBit State { get; private set; }
        public GoapAction Action { get; private set; }
        public float PathCost { get; private set; }
        

        internal GoapNodeBit(GoapNodeBit parent, GoapStateBit state, GoapAction action, float pathCost, GoapStateBit goalState, GoapPlanner planner)
        {
            this.Parent = parent;
            this.Action = action;
            this.State = state;
            this._goalState = goalState;
            this.PathCost = pathCost;            
            this._planner = planner;            

            //HeuristicCost = GoalState.Count;
            _heuristicCost = 0; // TODO count goals and set heuristic
            Cost = PathCost + _heuristicCost;
        }

        public List<GoapNodeBit> Expand(GoapAgent agent, Dictionary<GoapStateResLoc, int> resLocs)
        {
            List<GoapNodeBit> expandList = new List<GoapNodeBit>();
            foreach (GoapAction action in _planner.UsableActions)
            {
                if (action.Preconditions.InStateBit(this.State, resLocs.Count) && action.CheckProceduralPrecondition(agent))
                {
                    if (action.IsSpecific())
                    {
                        GoapStateBit newState = this.State.GetNewState(action.Effects, resLocs);
                        GoapNodeBit newNode = new GoapNodeBit(this, newState, action, this.PathCost + action.GetCost(), _goalState, _planner);
                        expandList.Add(newNode);
                    }
                    else
                    {
                        foreach (GoapAction sAction in action.GetSpecificActions(agent, this.State, this._goalState))
                        {
                            GoapStateBit newState = this.State.GetNewState(sAction.Effects, resLocs);
                            GoapNodeBit newNode = new GoapNodeBit(this, newState, sAction, this.PathCost, _goalState, _planner);
                            expandList.Add(newNode);
                        }
                    }
                }
            }
            return expandList;
        }

        //public List<GoapNodeBit> Expand2(GoapAgent agent)
        //{
        //    List<GoapNodeBit> expandList = new List<GoapNodeBit>();
        //    foreach (GoapAction action in _planner.UsableActions)
        //    {
        //        if (GoapPlanner.InState(action.Preconditions, this.State) && action.CheckProceduralPrecondition(agent))
        //        {
        //            if (action.IsSpecific())
        //            {
        //                GoapState newState = GoapPlanner.GetNewState(this.State, action.Effects);
        //                GoapNode newNode = new GoapNode(this, newState, action, this.PathCost, _goalState.Clone(), _planner);
        //                expandList.Add(newNode);
        //            }
        //            else
        //            {
        //                foreach(GoapAction sAction in action.GetSpecificActions(agent, this.State, this._goalState))
        //                {
        //                    GoapState newState = GoapPlanner.GetNewState(this.State, sAction.Effects);
        //                    GoapNode newNode = new GoapNode(this, newState, sAction, this.PathCost, _goalState.Clone(), _planner);
        //                    expandList.Add(newNode);
        //                }
        //            }
        //        }
        //    }
        //    return expandList;
        //}       
    }
}
