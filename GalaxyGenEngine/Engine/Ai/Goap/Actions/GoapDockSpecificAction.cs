using GalaxyGenEngine.Engine.Ai.Goap;
using GalaxyGenEngine.Engine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGenEngine.Engine.Ai.Goap.Actions
{
    public class GoapDockSpecificAction : GoapAction
    {
        private bool _requestSent = false;

        public GoapDockSpecificAction(long dockScId)
        {
            target = dockScId;
            GoapStateKey key = new GoapStateKey();            
            key.Type = GoapStateKeyEnum.String;
            key.String = "IsDocked";            
            addEffect(key, true);
            key.String = "DockedAt";           
            addEffect(key, dockScId);
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

        public override bool isSpecific()
        {
            return true;
        }
        public override List<GoapAction> GetSpecificActions(object agent)
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
            if (!ag.StateProvider.CurrentShipIsDocked && !_requestSent)
            {
                ag.ActionProvider.RequestDock();
                _requestSent = true;
            }
            return true;
        }

    }
}
