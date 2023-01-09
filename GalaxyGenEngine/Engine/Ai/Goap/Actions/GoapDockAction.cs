using GCEngine.Engine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCEngine.Engine.Ai.Goap.Actions
{
    public class GoapDockAction : GoapAction
    {
        private object _target;
        private bool _requestSent = false;
        

        public GoapDockAction(Int64 dockScId)
        {
            addPrecondition("DockedAt", 0L);   
            //addEffect("isDocked", true);
            addEffect("DockedAt", dockScId);
            _target = dockScId;
        }

        public override void reset()
        {
            _requestSent = false;
        }

        public override bool isDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return ag.StateProvider.CurrentShipIsDocked;
        }
    

        public override bool requiresInRange()
        {
            return true; 
        }

        public override bool CheckProceduralPrecondition()
        {
            target = _target;
            return true;
        }

        public override bool perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
           
            if (!ag.StateProvider.CurrentShipIsDocked && !_requestSent)
            {
                ag.ActionProvider.RequestDock();
                _requestSent = true;
            }

            return true;
        }
    }
}
