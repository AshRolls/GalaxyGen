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

        public GoapUndockAction()
        {            
            GoapStateKey key = new GoapStateKey(GoapStateKeyTypeEnum.StateName, GoapStateKeyStateNameEnum.IsDocked, new GoapStateKeyResLoc());
            addPrecondition(key, true);
            addEffect(key, false);
            key.StateName = GoapStateKeyStateNameEnum.DockedAt;
            addEffect(key, 0L);
        }

        public override void reset()
        {
            _requestSent = false;
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

        public override bool CheckProceduralPrecondition(object agent)
        {
            return true;
        }

        public override bool isSpecific()
        {
            return true;
        }

        public override List<GoapAction> GetSpecificActions(object agent, GoapState state)
        {
            return null;
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
