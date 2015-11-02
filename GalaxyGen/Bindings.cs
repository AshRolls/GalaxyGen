using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Extensions.Factory;
using GalaxyGen.Engine;
using GalaxyGen.Model;
using GalaxyGen.ViewModel;

namespace GalaxyGen
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
            kernel.Bind<IGalaxyCreator>().To<GalaxyCreator>();

            // Model bindings
            kernel.Bind<IPlanet>().To<Planet>();
            kernel.Bind<ISociety>().To<Society>();
            kernel.Bind<IMarket>().To<Market>();
            kernel.Bind<IAgent>().To<Agent>();

            // ViewModel bindings
            kernel.Bind<IMainViewModel>().To<MainViewModel>();
            kernel.Bind<IPlanetViewModel>().To<PlanetViewModel>();
            kernel.Bind<IPlanetViewModelFactory>().ToFactory();

            return kernel;
        }
    }
}
