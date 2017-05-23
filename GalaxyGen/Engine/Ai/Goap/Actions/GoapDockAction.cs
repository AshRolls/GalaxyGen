using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap.Actions
{
    public class GoapDockAction : GoapAction
    {
        public GoapDockAction()
        {
            addPrecondition("isDocked", false); // we need a tool to do this            
            addEffect("isDocked", true);
        }
        public override void reset()
        {

        }
        public override bool isDone()
        {
            return true;
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
            return true;
        }
    }
}
