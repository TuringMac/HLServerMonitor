using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ServerMonitor.Lib;
using ServerMonitor.NetworkAccess;

namespace ServerMonitor.WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IMonitor _monitor = new Monitor();
        Timer _timer = new Timer(10 * 60 * 1000);
        Timer _tmrCountdown = new Timer(1000);
        double time = 0;
        Server[] serversArgs;

        public MainWindow(Server[] pServers)
        {
            serversArgs = pServers;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _timer.Elapsed += _timer_Elapsed;
            _tmrCountdown.Elapsed += _tmrCountdown_Elapsed;
            foreach (Server srv in serversArgs)
            {
                _monitor.Add(srv);
            }
            _monitor.Refresh();
            DataContext = _monitor.ServerList;
            _tmrCountdown.Start();
            _timer.Start();
        }

        private void _tmrCountdown_Elapsed(object sender, ElapsedEventArgs e)
        {
            time += 1000;
            lblRemain.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                lblRemain.Content = TimeSpan.FromMilliseconds(_timer.Interval - time).ToString();
            }));
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            time = 0;
            _monitor.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _monitor.Refresh();
            time = 0;
            _timer.Stop();
            _timer.Start();
        }
    }
}
