using GalaxyGenEngine.Engine.Controllers;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGenEngine.Engine.Ai.Goap;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapLoadShipSpecificAction : GoapAction
    {
        private bool _requestSent = false;
        private ResourceQuantity _resQ;
        private long _curQ;

        public GoapLoadShipSpecificAction(ulong sourceStoreId, ulong destShipStoreId, ResourceQuantity resQ)
        {
            GoapStateKeyResLoc resLoc = new GoapStateKeyResLoc(resQ.Type, sourceStoreId);
            GoapStateKey key = new GoapStateKey(GoapStateKeyTypeEnum.Resource,GoapStateKeyStateNameEnum.None, resLoc);            
            //addPrecondition(key, (long)resQ.Quantity);
            addEffect(key, (0L - resQ.Quantity));
            resLoc.StoreId = destShipStoreId;
            key.ResourceLocation = resLoc;
            addEffect(key, resQ.Quantity);

            _resQ = resQ;
        }

        public override void reset()
        {
            _requestSent = false;
        }

        public override bool isDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return ag.StateProvider.CurrentShipResourceQuantity(_resQ.Type) >= _curQ + _resQ.Quantity;
        }

        public override bool requiresInRange()
        {
            return false;
        }

        public override bool isSpecific()
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

        public override bool perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;

            if (!_requestSent)
            {
                _curQ = ag.StateProvider.CurrentShipResourceQuantity(_resQ.Type);
                _requestSent = true;
                ag.ActionProvider.RequestLoadShip(_resQ);
            }

            return true;
        }
    }
}
