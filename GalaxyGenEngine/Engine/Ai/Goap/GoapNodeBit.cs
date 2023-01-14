using System.Collections.Generic;

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
            
            _heuristicCost = state.GetDifferenceCount(goalState, planner.ResLocs.Count);
            Cost = PathCost + _heuristicCost;
        }

        public List<GoapNodeBit> Expand(GoapAgent agent)
        {
            List<GoapNodeBit> expandList = new List<GoapNodeBit>();
            foreach (GoapAction action in _planner.UsableActions)
            {
                if (this.State.InStateBit(action.Preconditions, _planner.ResLocs.Count) && action.CheckProceduralPrecondition(agent))
                {
                    if (action.IsSpecific())
                    {
                        GoapStateBit newState = this.State.GetNewState(action.Effects, _planner.ResLocs.Count);
                        GoapNodeBit newNode = new GoapNodeBit(this, newState, action, this.PathCost + action.GetCost(), _goalState, _planner);
                        expandList.Add(newNode);
                    }
                    else
                    {
                        foreach (GoapAction sAction in action.GetSpecificActions(agent, this.State, this._goalState, _planner))
                        {
                            GoapStateBit newState = this.State.GetNewState(sAction.Effects, _planner.ResLocs.Count);
                            GoapNodeBit newNode = new(this, newState, sAction, this.PathCost + action.GetCost(), _goalState, _planner);
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
