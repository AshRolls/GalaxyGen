using System;

// interface needed only in Unity to use GetComponent and such features for generic agents
namespace GalaxyGen.Engine.Goap.Core
{
    public interface IReGoapAgentHelper
    {
        Type[] GetGenericArguments();
    }
}
