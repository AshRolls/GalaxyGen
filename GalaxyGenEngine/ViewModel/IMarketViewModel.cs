using GalaxyGenEngine.Model;

namespace GalaxyGenEngine.ViewModel
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
