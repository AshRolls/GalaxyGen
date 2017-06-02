using GalaxyGen.Engine.Controllers;
using GalaxyGen.Engine.Goap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap.Actions
{
    public class GoapUndockAction : GoapAction<string, object>
    {
        private string _name = "Goap Undock Action";
        private bool _requestSent = false;
        private bool _docked = true;

        public GoapUndockAction(Int64 undockScId)
        {
            addPrecondition("IsDocked", true);
            //addPrecondition("DockedAt", undockScId);

            addEffect("IsInSpace", true);
            addEffect("IsDocked", false);
            //addEffect("DockedAt", 0);
        }

        public override void reset()
        {
            _requestSent = false;
            _docked = true;
        }

        public override bool IsDone()
        {
            return _docked == false;
        }
    

        public override bool RequiresInRange()
        {
            return false; 
        }

        public override bool checkProceduralPrecondition(object agent)
        {
            // set target?
            return true;
        }

        public override bool Perform(IReGoapActionSettings<string, object> settings, ReGoapState<string, object> goalState)
        {            
            if (!agent.StateProvider.CurrentShipIsDocked)
                _docked = false;
            else if (agent.StateProvider.CurrentShipIsDocked && !_requestSent)
            {
                _requestSent = true;
                agent.ActionProvider.RequestUndock();
            }

            // TODO add a number of count before retry undock 
            return true;
        }

        public override string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
