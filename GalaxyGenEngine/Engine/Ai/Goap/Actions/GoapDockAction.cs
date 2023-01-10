using GalaxyGenEngine.Engine.Ai.Goap;
using GalaxyGenEngine.Engine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapDockAction : GoapAction
    {
        private object _target;
        private bool _requestSent = false;
        

        public GoapDockAction(Int64 dockScId)
        {
            GoapStateKey key = new GoapStateKey();
            key.Type = GoapStateKeyEnum.String;
            key.String = "DockedAt";

            addPrecondition(key, 0L);               
            addEffect(key, dockScId);
            
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
