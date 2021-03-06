﻿using GalaxyGen.Engine.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap.Actions
{
    public class GoapUndockAction : GoapAction
    {
        private bool _requestSent = false;
        private bool _docked = true;

        public GoapUndockAction(Int64 undockScId)
        {
            addPrecondition("isDocked", true);
            addPrecondition("DockedAt", undockScId);
            addEffect("isDocked", false);
            addEffect("DockedAt", 0);
        }

        public override void reset()
        {
            _requestSent = false;
            _docked = true;
        }

        public override bool isDone()
        {
            return _docked == false;
        }
    

        public override bool requiresInRange()
        {
            return false; 
        }

        public override bool checkProceduralPrecondition(object agent)
        {
            // set target?
            return true;
        }

        public override bool perform(object agent)
        {
            GoapAgent ag = (GoapAgent)agent;

            if (!ag.StateProvider.CurrentShipIsDocked)
                _docked = false;
            else if (ag.StateProvider.CurrentShipIsDocked && !_requestSent)
            {
                _requestSent = true;
                ag.ActionProvider.RequestUndock();
            }

            // TODO add a number of count before retry undock 
            return true;
        }
    }
}
