using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapLoadShipSpecificAction : GoapAction
    {
        private ResourceQuantity _resQ;
        private long _curQ;

        public GoapLoadShipSpecificAction(ulong sourceStoreId, ulong destShipStoreId, ResourceQuantity resQ)
        {
            GoapStateKeyResLoc resLoc = new(resQ.Type, sourceStoreId);
            GoapStateKey key = new(GoapStateKeyTypeEnum.ResourceQty,GoapStateKeyStateNameEnum.None, resLoc, ResourceTypeEnum.NotSet);            
            //addPrecondition(key, (long)resQ.Quantity);
            addEffect(key, (0L - resQ.Quantity));
            resLoc.StoreId = destShipStoreId;
            key.ResourceLocation = resLoc;
            addEffect(key, resQ.Quantity);
            key = new(GoapStateKeyTypeEnum.AllowedResource, GoapStateKeyStateNameEnum.None, new GoapStateKeyResLoc(), resQ.Type);
            addEffect(key, 0);
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
        public override List<GoapAction> GetSpecificActions(object agent, GoapState state, GoapState goal)
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
