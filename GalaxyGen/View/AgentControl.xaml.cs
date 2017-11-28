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
    /// Interaction logic for AgentControl.xaml
    /// </summary>
    public partial class AgentControl : UserControl
    {
        public AgentControl()
        {
            InitializeComponent();
        }

        public IAgentViewModel AgentVm
        {
            get { return (IAgentViewModel)GetValue(AgentVmProperty); }
            set { SetValue(AgentVmProperty, value); }
        }

        public static readonly DependencyProperty AgentVmProperty =
            DependencyProperty.Register("AgentVm", typeof(IAgentViewModel), typeof(AgentControl));
    }
}
