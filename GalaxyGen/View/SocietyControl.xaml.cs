using GCEngine.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace GalaxyGen
{
    /// <summary>
    /// Interaction logic for SocietyControl.xaml
    /// </summary>
    public partial class SocietyControl : UserControl
    {
        public SocietyControl()
        {
            InitializeComponent();
        }

        public ISocietyViewModel SocietyVm
        {
            get { return (ISocietyViewModel)GetValue(SocietyVmProperty); }
            set { SetValue(SocietyVmProperty, value); }
        }

        public static readonly DependencyProperty SocietyVmProperty =
            DependencyProperty.Register("SocietyVm", typeof(ISocietyViewModel), typeof(SocietyControl));
    }
}
