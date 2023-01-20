using GalaxyGenCore.Resources;
using System.Collections.Generic;


namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapCreateSellOrderSpecificAction : GoapAction
    {
        private ResourceQuantity _resQ;

        public GoapCreateSellOrderSpecificAction(ResourceQuantity resQ, ulong sourceStoreId, ulong destPlanetScId, (bool newAllowedRes, int allowedIdx) allowedStatus, GoapPlanner planner)
        {                        
            GoapStateResLoc resLoc = new(resQ.Type, sourceStoreId);
            AddResEffect(resLoc, (0L - resQ.Quantity), planner);
            resLoc = new(resQ.Type, destPlanetScId);
            AddResEffect(resLoc, resQ.Quantity, planner);
            if (allowedStatus.newAllowedRes) AddEffect((GoapStateBitFlagsEnum)((ulong)GoapStateBitFlagsEnum.AllowedRes1 << allowedStatus.allowedIdx), (ulong)resQ.Type);
            _resQ = resQ;
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

        public override List<GoapAction> GetSpecificActions(object agent, GoapStateBit state, GoapStateBit goal, GoapPlanner planner)
        {
            return null;
        }

        public override bool Perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            if (ag.StateProvider.CurrentShipIsDocked) ag.ActionProvider.RequestCreateSellOrder(_resQ);
            return true;
        }
    }
}
