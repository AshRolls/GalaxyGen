using Akka.Actor;
using GalaxyGenEngine.Framework;
using GalaxyGenEngine.ViewModel;
using InteractiveDataDisplay.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;


namespace GalaxyGen
{
    /// <summary>
    /// Interaction logic for SolarSystemControl.xaml
    /// </summary>
    public partial class PlanetControl : UserControl
    {
        private Timer _graphDataTimer;
        private Queue<(int, double, double)> _values;        
        private int _count;
        private LineGraph _lineGraphX;
        private LineGraph _lineGraphY;
        private ulong _curId;

        public PlanetControl()
        {
            InitializeComponent();
            _values = new();            
            _count = 0;
            _graphDataTimer = new Timer(50);
            _graphDataTimer.Elapsed += GraphDataTimer_Elapsed;
            _graphDataTimer.Start();
            _lineGraphX = new();
            _lineGraphY = new();
            lines.Children.Add(_lineGraphX);
            lines.Children.Add(_lineGraphY);
        }

        private void GraphDataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action action = () => {
                if (PlanetVm != null)
                {
                    if (_curId != PlanetVm.Model.StarChartId)
                    {
                        _values.Clear();
                        _curId = PlanetVm.Model.StarChartId;
                    }
                    Vector2 pos = PlanetVm.Position;
                    _count++;
                    _values.Enqueue((_count, pos.X, pos.Y));                    
                    while (_values.Count > 1000) { _ = _values.Dequeue(); }
                    var countVals = _values.Select(x => x.Item1);
                    _lineGraphX.Plot(countVals, _values.Select(x => x.Item2));
                    _lineGraphY.Plot(countVals, _values.Select(x => x.Item3));
                }
            };
            Dispatcher.BeginInvoke(action);
        }

        public ISolarSystemViewModel SolarSystemVm
        {
            get { return (ISolarSystemViewModel)GetValue(SolarSystemVmProperty); }
            set { SetValue(SolarSystemVmProperty, value); }
        }

        public static readonly DependencyProperty SolarSystemVmProperty =
            DependencyProperty.Register("SolarSystemVm", typeof(ISolarSystemViewModel), typeof(PlanetControl));

        public IPlanetViewModel PlanetVm
        {
            get { return (IPlanetViewModel)GetValue(PlanetVmProperty); }
            set { SetValue(PlanetVmProperty, value); }
        }

        public static readonly DependencyProperty PlanetVmProperty =
            DependencyProperty.Register("PlanetVm", typeof(IPlanetViewModel), typeof(PlanetControl));
    }
}

