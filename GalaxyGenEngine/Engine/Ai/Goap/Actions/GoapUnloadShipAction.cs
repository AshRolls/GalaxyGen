using GalaxyGenEngine.Engine.Controllers;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyGenEngine.Engine.Ai.Goap;

//namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
//{
//    public class GoapUnloadShipAction : GoapAction
//    {
//        private bool _requestSent = false;
//        private ResourceQuantity _resQ;
//        private ulong _curQ;
//        private long _dockScId;

//        public GoapUnloadShipAction(long dockScId, long sourceShipStoreId, long destStoreId, ResourceQuantity resQ)
//        {            
//            GoapStateKey key = new GoapStateKey();
//            key.Type = GoapStateKeyEnum.String;
//            key.String = "DockedAt";
//            addPrecondition(key, dockScId);
            
//            key = new GoapStateKey();
//            key.Type = GoapStateKeyEnum.Resource;
//            key.ResType = resQ.Type;
//            key.StoreId = sourceShipStoreId;
//            addPrecondition(key, (long)resQ.Quantity);
//            addEffect(key, (0L - (long)resQ.Quantity));
//            key.StoreId = destStoreId;
//            addEffect(key, (long)resQ.Quantity);
            
//            _resQ = resQ;                       
//        }

//        public override void reset()
//        {
//            _requestSent = false;
//        }

//        public override bool isDone(object agent)
//        {
//            GoapAgent ag = (GoapAgent)agent;
//            return ag.StateProvider.PlanetResourceQuantity(_dockScId, _resQ.Type) >= _curQ;          
//        }


//        public override bool requiresInRange()
//        {
//            return false; 
//        }

//        public override bool CheckProceduralPrecondition()
//        {
//            return true;
//        }

//        public override bool perform(object agent)
//        {
//            GoapAgent ag = (GoapAgent)agent;

//            if (!_requestSent)
//            {
//                _curQ = ag.StateProvider.PlanetResourceQuantity(_dockScId, _resQ.Type);
//                _requestSent = true; 
//                ag.ActionProvider.RequestUnloadShip(_resQ);
//            }

//            return true;
//        }
//    }
//}
