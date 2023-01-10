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
    public class GoapLoadShipAction : GoapAction
    {
        private bool _requestSent = false;
        private ResourceQuantity _resQ;
        private ulong _curQ;

        public GoapLoadShipAction(long dockScId, long sourceStoreId, long destShipStoreId, ResourceQuantity resQ)
        {            
            GoapStateKey key = new GoapStateKey();
            key.Type = GoapStateKeyEnum.String;
            key.String = "DockedAt";
            addPrecondition(key, dockScId);
            
            key = new GoapStateKey();
            key.Type = GoapStateKeyEnum.Resource;
            key.ResType = resQ.Type;
            key.StoreId = sourceStoreId;
            addPrecondition(key, resQ.Quantity);
            addEffect(key, (0 - resQ.Quantity));
            key.StoreId = destShipStoreId;
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
            return ag.StateProvider.CurrentShipResourceQuantity(_resQ.Type) >= _resQ.Quantity;          
        }


        public override bool requiresInRange()
        {
            return false; 
        }

        public override bool CheckProceduralPrecondition()
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
