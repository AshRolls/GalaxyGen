using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap.Goals
{
    public class GoapUndockGoal : GoapGoal<string, object>
    {
        public GoapUndockGoal()
        {
            goal.Set("IsInSpace", true);
            //goal.Set("DockedAt", 0);
        }
    }
}
