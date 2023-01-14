using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapLoadShipSpecificAction : GoapAction
    {
        private ResourceQuantity _resQ;
        private long _curQ;

        public GoapLoadShipSpecificAction(ulong sourceStoreId, ulong destShipStoreId, ResourceQuantity resQ, GoapPlanner planner)
        {
            GoapStateResLoc resLoc = new(resQ.Type, sourceStoreId);                        
            addResEffect(resLoc, (0L - resQ.Quantity), planner);
            resLoc = new(resQ.Type, destShipStoreId);                         
            addResEffect(resLoc, resQ.Quantity, planner);
            
            
            //key = new(GoapStateKeyTypeEnum.AllowedResource, GoapStateKeyStateNameEnum.None, new GoapStateKeyResLoc(), resQ.Type);
            //addEffect(key, 0);
            //TODO allowed resource effect here
            _resQ = resQ;
        }

        public override void Reset()
        {
        }

        public override bool IsDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return ag.StateProvider.CurrentShipResourceQuantity(_resQ.Type) >= _curQ + _resQ.Quantity;
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
            _curQ = ag.StateProvider.CurrentShipResourceQuantity(_resQ.Type);
            ag.ActionProvider.RequestLoadShip(_resQ);
            return true;
        }
    }
}
