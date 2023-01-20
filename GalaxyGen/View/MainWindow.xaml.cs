using Ninject;
using System.Windows;
using GalaxyGenEngine.ViewModel;
using System.Windows.Controls;
using System;
using static GalaxyGen.Raylib.SolarSystemVis;
using GalaxyGen.Raylib;
using GalaxyGenEngine.Model;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;

namespace GalaxyGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SolarSystemVis _visualiser;
        private Timer _refreshTimer;
        private IMainGalaxyViewModel _mvm;

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
            _mvm = kernel.Get<IMainGalaxyViewModel>();
            this.DataContext = _mvm;
            
            _visualiser = new();
            Task.Run(() => _visualiser.StartVisualiser());
            setupAndStartTimer();
        }

        private void setupAndStartTimer()
        {
            _refreshTimer = new Timer(50);
            _refreshTimer.Elapsed += _refreshTimer_Elapsed;
            _refreshTimer.Start();
        }

        private void _refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_mvm.SelectedSolarSystemVm != null)
            {
                lock (_mvm.SelectedSolarSystemVm)
                {
                    (double, double)[] shipsArray = new (double, double)[_mvm.SelectedSolarSystemVm.Ships.Count];
                    int i = 0;
                    foreach (IShipViewModel shipVm in _mvm.SelectedSolarSystemVm.Ships.ToList())
                    {
                        shipsArray[i++] = (shipVm.PositionX, shipVm.PositionY);
                    }
                    RenderArray rA = new(1, shipsArray);
                    _visualiser.AddRenderArray(rA);
                }                
            }
        }

        private Boolean _autoScroll = true;
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scroll = sender as ScrollViewer;
            // User scroll event : set or unset auto-scroll mode
            if (e.ExtentHeightChange == 0)
            {   // Content unchanged : user scroll event
                if (scroll.VerticalOffset == scroll.ScrollableHeight)
                {   // Scroll bar is in bottom
                    // Set auto-scroll mode
                    _autoScroll = true;
                }
                else
                {   // Scroll bar isn't in bottom
                    // Unset auto-scroll mode
                    _autoScroll = false;
                }
            }

            // Content scroll event : auto-scroll eventually
            if (_autoScroll && e.ExtentHeightChange != 0)
            {   // Content changed and auto-scroll mode set
                // Autoscroll
                scroll.ScrollToVerticalOffset(scroll.ExtentHeight);
            }
        }
    }
}
