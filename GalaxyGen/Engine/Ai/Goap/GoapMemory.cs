using GalaxyGen.Engine.Goap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Ai.Goap
{
    public class GoapMemory<T, W> : IReGoapMemory<T, W>
    {
        protected ReGoapState<T, W> state;

        public GoapMemory()
        {
            state = ReGoapState<T, W>.Instantiate();
        }

        protected virtual void OnDestroy()
        {
            state.Recycle();
        }

        public virtual ReGoapState<T, W> GetWorldState()
        {
            return state;
        }
    }
}
