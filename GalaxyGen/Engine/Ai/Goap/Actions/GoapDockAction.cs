using GalaxyGen.Engine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap.Actions
{
    public class GoapDockAction : GoapAction
    {
        private bool _docked = false;
        private bool _requestSent = false;
        private Int64 _target;

        public GoapDockAction(Int64 dockScId)
        {
            addPrecondition("DockedAt", 0);   
            //addEffect("isDocked", true);
            addEffect("DockedAt", dockScId);
            _target = dockScId;
        }

        public override void reset()
        {
            _docked = false;
            _requestSent = false;
        }

        public override bool isDone()
        {
            return _docked == true;
        }
    

        public override bool requiresInRange()
        {
            return true; 
        }

        public override bool checkProceduralPrecondition(object agent)
        {
            target = _target;
            return true;
        }

        public override bool perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;

            if (ag.StateProvider.CurrentShipIsDocked)
                _docked = true;
            else if (!ag.StateProvider.CurrentShipIsDocked && !_requestSent)
            {
                ag.ActionProvider.RequestDock();
                _requestSent = true;
            }

            return true;
        }
    }
}
