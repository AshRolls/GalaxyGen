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
