namespace GalaxyGen.Engine.Goap.Core
{
    public interface IReGoapMemory<T, W>
    {
        ReGoapState<T, W> GetWorldState();
    }
}