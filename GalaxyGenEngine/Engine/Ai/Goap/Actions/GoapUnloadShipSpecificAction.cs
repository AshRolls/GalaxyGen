using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapUnloadShipSpecificAction : GoapAction
    {
        private bool _requestSent = false;
        private ResourceQuantity _resQ;
        private long _curQ;        

        public GoapUnloadShipSpecificAction(ulong sourceShipStoreId, ulong destStoreId, ResourceQuantity resQ)
        {
            GoapStateKeyResLoc resLoc = new GoapStateKeyResLoc(resQ.Type, sourceShipStoreId);
            GoapStateKey key = new GoapStateKey(GoapStateKeyTypeEnum.Resource, GoapStateKeyStateNameEnum.None, resLoc);            
            addEffect(key, (0L - resQ.Quantity));
            resLoc.StoreId = destStoreId;
            key.ResourceLocation = resLoc;
            addEffect(key, resQ.Quantity);

            _resQ = resQ;
        }

        public override void Reset()
        {
            _requestSent = false;
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
        public override List<GoapAction> GetSpecificActions(object agent, GoapState state)
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

            if (!_requestSent)
            {
                _curQ = ag.StateProvider.CurrentPlanetResourceQuantity(_resQ.Type);
                _requestSent = true;
                ag.ActionProvider.RequestUnloadShip(_resQ);
            }

            return true;
        }
    }
}
