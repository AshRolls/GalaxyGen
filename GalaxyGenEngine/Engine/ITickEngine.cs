using GalaxyGenEngine.ViewModel;
using System;

namespace GalaxyGenEngine.Engine
{
    public interface ITickEngine
    {
        void SetupTickEngine(IGalaxyViewModel state, ITextOutputViewModel textOutput);
        void Run(bool maxRate);
        void Stop();
    }
}