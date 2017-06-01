using GalaxyGen.Engine.Controllers;
using GalaxyGenCore.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap.Actions
{
    public class GoapLoadShipAction : GoapAction
    {
        private bool _loaded = false;
        private bool _requestSent = false;
        private ResourceQuantity _resourceQ;

        public GoapLoadShipAction(Int64 dockScId, ResourceQuantity resQ)
        {
            addPrecondition("isDocked", true);
            addPrecondition("DockedAt", dockScId);
            addResource((Int64)resQ.Type, (Int64)resQ.Quantity);
            _resourceQ = resQ;           
        }

        public override void reset()
        {
            _loaded = false;
            _requestSent = false;
        }

        public override bool isDone()
        {
            return _loaded == true;
        }
    

        public override bool requiresInRange()
        {
            return false; 
        }

        public override bool checkProceduralPrecondition(object agent)
        {
            return true;
        }

        public override bool perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;

            //if (ag.StateProvider.CurrentShipResourceQuantity(_resourceQ.Type) >= _resourceQ.Quantity)            
            //    _loaded = true;
            //else if (!_requestSent)
            //{
            //    ag.ActionProvider.RequestLoadShip(_resourceQ);
            //    _requestSent = true;
            //}
            _loaded = true;
            ag.ActionProvider.RequestLoadShip(_resourceQ);

            return true;
        }
    }
}
