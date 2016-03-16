using GalaxyGen.ViewModel;
using System;

namespace GalaxyGen.Engine
{
    public interface ITickEngine
    {
        void SetupTickEngine(IGalaxyViewModel state);
        void RunNTick(Int32 numberOfTicks);
    }
}