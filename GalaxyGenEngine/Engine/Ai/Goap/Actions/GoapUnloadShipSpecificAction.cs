using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapUnloadShipSpecificAction : GoapAction
    {
        private ResourceQuantity _resQ;
        private long _curQ;        

        public GoapUnloadShipSpecificAction(ulong sourceShipStoreId, ulong destStoreId, ResourceQuantity resQ, (bool newAllowedRes, int allowedIdx) allowedStatus, GoapPlanner planner)
        {
            GoapStateResLoc resLoc = new(resQ.Type, sourceShipStoreId);
            AddResEffect(resLoc, (0L - resQ.Quantity), planner);
            resLoc = new(resQ.Type, destStoreId);            
            AddResEffect(resLoc, resQ.Quantity, planner);            
            if (allowedStatus.newAllowedRes) AddEffect((GoapStateBitFlagsEnum)((ulong)GoapStateBitFlagsEnum.AllowedRes1 << allowedStatus.allowedIdx), (ulong)resQ.Type); 
            _resQ = resQ;
        }

        public override void Reset()
        {
        }

        public override bool IsDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return ag.StateProvider.CurrentPlanetResourceQuantity(_resQ.Type) >= _curQ + _resQ.Quantity;
        }


        public override bool RequiresInRange()
        {
            return false;
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
            _curQ = ag.StateProvider.CurrentPlanetResourceQuantity(_resQ.Type);
            ag.ActionProvider.RequestUnloadShip(_resQ);
            return true;
        }
    }
}
