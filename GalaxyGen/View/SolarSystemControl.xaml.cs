using GCEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
