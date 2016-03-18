using GalaxyGen.ViewModel;
using System;

namespace GalaxyGen.Engine
{
    public interface ITickEngine
    {
        void SetupTickEngine(IGalaxyViewModel state, ITextOutputViewModel textOutput);
        void Run();
        void Stop();
    }
}