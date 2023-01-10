using Ninject;
using System.Windows;
using GalaxyGenEngine.ViewModel;

namespace GalaxyGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetupBackEnd();
        }

        private void SetupBackEnd()
        {
            StandardKernel kernel = GalaxyGenEngine.Bindings.Kernel;
            IMainGalaxyViewModel mvm = kernel.Get<IMainGalaxyViewModel>();
            this.DataContext = mvm;
        }
    }
}
