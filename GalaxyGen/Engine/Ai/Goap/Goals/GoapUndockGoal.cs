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
            goal.Set("IsDocked", false);
            goal.Set("DockedAt", 0);
        }
    }
}
