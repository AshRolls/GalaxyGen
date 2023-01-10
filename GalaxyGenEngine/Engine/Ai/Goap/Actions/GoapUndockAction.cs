using GalaxyGenEngine.Engine.Ai.Goap;
using GalaxyGenEngine.Engine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapUndockAction : GoapAction
    {
        private bool _requestSent = false;
        private bool _docked = true;

        public GoapUndockAction(Int64 undockScId)
        {            
            GoapStateKey key = new GoapStateKey();
            key.Type = GoapStateKeyEnum.String;
            key.String = "DockedAt";
            addPrecondition(key, undockScId);            
            addEffect(key, 0L);
        }

        public override void reset()
        {
            _requestSent = false;
            _docked = true;
        }

        public override bool isDone(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;
            return !ag.StateProvider.CurrentShipIsDocked;
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

            if (ag.StateProvider.CurrentShipIsDocked && !_requestSent)
            {
                _requestSent = true;
                ag.ActionProvider.RequestUndock();
            }

            // TODO add a number of count before retry undock 
            return true;
        }
    }
}
