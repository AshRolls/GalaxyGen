using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap.Actions
{
    public class GoapUndockAction : GoapAction
    {
        private bool _docked = true;

        public GoapUndockAction()
        {
            addPrecondition("isDocked", true); // we need a tool to do this            
            addEffect("isDocked", false);
        }

        public override void reset()
        {
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
            _docked = false;
            return true;
        }
    }
}
