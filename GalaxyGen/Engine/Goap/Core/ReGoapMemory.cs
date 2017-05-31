using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyGen.Engine.Goap.Core
{
    public class ReGoapMemory<T, W> : IReGoapMemory<T, W>
    {
        protected ReGoapState<T, W> state;

        public ReGoapMemory()
        {
            state = ReGoapState<T, W>.Instantiate();
        }

        public virtual ReGoapState<T, W> GetWorldState()
        {
            return state;
        }
    }
}
