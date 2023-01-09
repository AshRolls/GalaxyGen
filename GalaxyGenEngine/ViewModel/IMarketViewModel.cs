using GCEngine.Model;

namespace GCEngine.ViewModel
{
    public interface IMarketViewModel
    {
        Market Model { get; set; }
    }

    public interface IMarketViewModelFactory
    {
        IMarketViewModel CreateMarketViewModel();
    }
}
