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

        public GoapDockAction()
        {
            addPrecondition("isDocked", false);   
            addEffect("isDocked", true);
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
            target = 4;
            return true;
        }

        public override bool perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;

            if (ag.stateProvider.CurrentShipIsDocked)
                _docked = true;
            else if (!ag.stateProvider.CurrentShipIsDocked && !_requestSent)
            {
                ag.actionProvider.RequestDock();
                _requestSent = true;
            }

            return true;
        }
    }
}
