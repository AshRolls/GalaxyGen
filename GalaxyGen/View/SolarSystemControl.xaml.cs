using GCEngine.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace GalaxyGen
{
    /// <summary>
    /// Interaction logic for SolarSystemControl.xaml
    /// </summary>
    public partial class SolarSystemControl : UserControl
    {
        public SolarSystemControl()
        {
            InitializeComponent();
        }

        public ISolarSystemViewModel SolarSystemVm
        {
            get { return (ISolarSystemViewModel)GetValue(SolarSystemVmProperty); }
            set { SetValue(SolarSystemVmProperty, value); }
        }

        public static readonly DependencyProperty SolarSystemVmProperty =
            DependencyProperty.Register("SolarSystemVm", typeof(ISolarSystemViewModel), typeof(SolarSystemControl));

        public IPlanetViewModel PlanetVm
        {
            get { return (IPlanetViewModel)GetValue(PlanetVmProperty); }
            set { SetValue(PlanetVmProperty, value); }
        }

        public static readonly DependencyProperty PlanetVmProperty =
            DependencyProperty.Register("PlanetVm", typeof(IPlanetViewModel), typeof(SolarSystemControl));
    }
}
