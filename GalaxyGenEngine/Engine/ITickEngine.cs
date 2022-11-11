using GCEngine.ViewModel;
using System;

namespace GCEngine.Engine
{
    public interface ITickEngine
    {
        void SetupTickEngine(IGalaxyViewModel state, ITextOutputViewModel textOutput);
        void Run(bool maxRate);
        void Stop();
    }
}