using GalaxyGenCore.Resources;
using System.Collections.Generic;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapUnloadShipSpecificAction : GoapAction
    {
        private ResourceQuantity _resQ;
        private long _curQ;        

        public GoapUnloadShipSpecificAction(ulong sourceShipStoreId, ulong destStoreId, ResourceQuantity resQ)
        {
            GoapStateResLoc resLoc = new(resQ.Type, sourceShipStoreId);
            //addResEffect(resLoc, (0L - resQ.Quantity));
            resLoc = new(resQ.Type, destStoreId);            
            //addResEffect(resLoc, resQ.Quantity);
            
            //TODO add allowed resources here
            //key = new(GoapStateKeyTypeEnum.AllowedResource, GoapStateKeyStateNameEnum.None, new GoapStateKeyResLoc(), resQ.Type);
            //addEffect(key, 0);

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
        public override List<GoapAction> GetSpecificActions(object agent, GoapStateBit state, GoapStateBit goal)
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
