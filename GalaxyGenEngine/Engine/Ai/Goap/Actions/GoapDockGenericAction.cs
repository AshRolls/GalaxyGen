using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapDockGenericAction : GoapAction
    {   
        public GoapDockGenericAction(HashSet<ulong> possibleDocks)
        {
            _targets = possibleDocks;
            addPrecondition(GoapStateBitFlagsEnum.IsDocked, 0UL);
            addPrecondition(GoapStateBitFlagsEnum.DockedAt, 0UL);
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

        public override List<GoapAction> GetSpecificActions(object agent, GoapStateBit state, GoapStateBit goal)
        {
            List<GoapAction> actions = new();
            foreach (ulong t in _targets)
            {
                GoapDockSpecificAction a = new(t);
                actions.Add(a);
            }
            return actions;
        }

        public override bool Perform(object agent)
        {            
            return true;
        }

        private static HashSet<ulong> _targets;
        public static HashSet<ulong> Targets
        {
            get { return _targets; }
        }
    }
}
