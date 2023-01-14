using GalaxyGenCore.Resources;
using System.Collections.Generic;


namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapUndockAction : GoapAction
    {
        public GoapUndockAction()
        {                        
            addPrecondition(GoapStateBitFlagsEnum.IsDocked, 1UL);
            addEffect(GoapStateBitFlagsEnum.IsDocked, 0L);            
            addEffect(GoapStateBitFlagsEnum.DockedAt, 0L);
        }

        public override void Reset()
        {
        }

        public override bool IsDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return !ag.StateProvider.CurrentShipIsDocked;
        }
    

        public override bool RequiresInRange()
        {
            return false; 
        }

        public override bool CheckProceduralPrecondition(object agent)
        {
            return true;
        }

        public override bool IsSpecific()
        {
            return true;
        }

        public override List<GoapAction> GetSpecificActions(object agent, GoapStateBit state, GoapStateBit goal)
        {
            return null;
        }

        public override bool Perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            if (ag.StateProvider.CurrentShipIsDocked) ag.ActionProvider.RequestUndock();
            return true;
        }
    }
}
