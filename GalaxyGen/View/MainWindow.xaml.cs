using Ninject;
using System.Windows;
using GalaxyGenEngine.ViewModel;
using System.Windows.Controls;
using System;

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

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scroll = sender as ScrollViewer;
            if (scroll != null)
            {
                scroll.ScrollToEnd();
            }
        }
    }
}
