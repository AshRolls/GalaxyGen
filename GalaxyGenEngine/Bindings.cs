using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Extensions.Factory;
using GalaxyGenEngine.Engine;
using GalaxyGenEngine.Model;
using GalaxyGenEngine.ViewModel;

namespace GalaxyGenEngine
{
    public class Bindings
    {
        static StandardKernel Kernel_Var;
        static Object Kernel_Lock = new Object();
        public static StandardKernel Kernel
        {
            get
            {
                lock (Kernel_Lock)
                {
                    if (null == Kernel_Var)
                    {
                        Kernel_Var = GetNinjectKernel();
                    }
                }
                return Kernel_Var;
            }

            set
            {
                Kernel_Var = value;
            }
        }

        private static StandardKernel GetNinjectKernel()
        {
            StandardKernel kernel = new StandardKernel();

            // Engine bindings
            kernel.Bind<ITickEngine>().To<TickEngine>();
            kernel.Bind<IGalaxyPopulator>().To<GalaxyPopulator>();

            // Framework bindings
           

            // ViewModel bindings
            kernel.Bind<IMainGalaxyViewModel>().To<MainGalaxyViewModel>();
            kernel.Bind<ITextOutputViewModel>().To<TextOutputViewModel>().InSingletonScope();
            kernel.Bind<IGalaxyViewModel>().To<GalaxyViewModel>();
            kernel.Bind<IGalaxyViewModelFactory>().ToFactory();
            kernel.Bind<ISolarSystemViewModel>().To<SolarSystemViewModel>();
            kernel.Bind<ISolarSystemViewModelFactory>().ToFactory();
            kernel.Bind<IPlanetViewModel>().To<PlanetViewModel>();
            kernel.Bind<IPlanetViewModelFactory>().ToFactory();
            kernel.Bind<ISocietyViewModel>().To<SocietyViewModel>();
            kernel.Bind<ISocietyViewModelFactory>().ToFactory();
            kernel.Bind<IMarketViewModel>().To<MarketViewModel>();
            kernel.Bind<IMarketViewModelFactory>().ToFactory();
            kernel.Bind<IAgentViewModel>().To<AgentViewModel>();
            kernel.Bind<IAgentViewModelFactory>().ToFactory();
            kernel.Bind<IProducerViewModel>().To<ProducerViewModel>();
            kernel.Bind<IProducerViewModelFactory>().ToFactory();
            kernel.Bind<IResourceTypeViewModel>().To<ResourceTypeViewModel>();
            kernel.Bind<IResourceTypeViewModelFactory>().ToFactory();
            kernel.Bind<IShipViewModel>().To<ShipViewModel>();
            kernel.Bind<IShipViewModelFactory>().ToFactory();

            return kernel;
        }
    }
}
