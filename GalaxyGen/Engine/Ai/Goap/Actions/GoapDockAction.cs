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

        public GoapDockAction()
        {
            addPrecondition("isDocked", false);   
            addEffect("isDocked", true);
        }

        public override void reset()
        {
            _docked = false;
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
            if (ag.dataProvider.RequestDock())
            {
                _docked = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
