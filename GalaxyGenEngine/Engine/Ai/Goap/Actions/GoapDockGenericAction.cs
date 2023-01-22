using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapDockGenericAction : GoapAction
    {   
        public GoapDockGenericAction(HashSet<ulong> possibleDocks)
        {
            _targets = possibleDocks;
            AddPrecondition(GoapStateBitFlagsEnum.IsDocked, 0UL);
            AddPrecondition(GoapStateBitFlagsEnum.DockedAt, 0UL);
        }

        public override void Reset()
        {
        }

        public override bool IsDone(object agent)
        {
            return true;
        }
    
        public override bool RequiresInRange()
        {
            return true; 
        }
       
        public override bool CheckProceduralPrecondition(object agent)
        {
            return true;
        }

        public override bool IsSpecific()
        {
            return false;
        }  

        public override List<GoapAction> GetSpecificActions(object agent, GoapStateBit state, GoapStateBit goal, GoapPlanner planner)
        {
            List<GoapAction> actions = new();
            foreach (ulong t in _targets)
            {
                (bool allowed, (bool, int) allowedStatus) = state.IsAllowedDestination(t);
                if (allowed)
                {
                    GoapDockSpecificAction a = new(t, allowedStatus);
                    actions.Add(a);
                }                
            }
            return actions;
        }

        public override bool Perform(object agent)
        {            
            return true;
        }

        private HashSet<ulong> _targets;
        public HashSet<ulong> Targets
        {
            get { return _targets; }
        }
    }
}
