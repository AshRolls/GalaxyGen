using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapDockSpecificAction : GoapAction
    {
        public GoapDockSpecificAction(ulong dockScId, (bool newAllowedDest, int allowedIdx) allowedStatus)
        {
            target = dockScId;
            AddEffect(GoapStateBitFlagsEnum.IsDocked, 1UL);
            AddEffect(GoapStateBitFlagsEnum.DockedAt, dockScId);
            if (allowedStatus.newAllowedDest) AddEffect((GoapStateBitFlagsEnum)((ulong)GoapStateBitFlagsEnum.AllowedLoc1 << allowedStatus.allowedIdx), dockScId);
        }

        public override void Reset()
        {
        }

        public override bool IsDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return ag.StateProvider.CurrentShipIsDocked;
        }
    
        public override bool RequiresInRange()
        {
            return true; 
        }

        public override bool IsSpecific()
        {
            return true;
        }
        public override List<GoapAction> GetSpecificActions(object agent, GoapStateBit state, GoapStateBit goal, GoapPlanner planner)
        {
            return null;
        }

        public override bool CheckProceduralPrecondition(object agent)
        {
            return true;
        }

        public override bool Perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;           
            if (!ag.StateProvider.CurrentShipIsDocked) ag.ActionProvider.RequestDock();
            return true;
        }

    }
}
