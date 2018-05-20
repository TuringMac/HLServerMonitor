using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using ServerMonitor.NetworkAccess;

namespace ServerMonitor.WPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            List<Server> servers = new List<Server>();
            if (e.Args.Length > 0)
                foreach (var srvip in e.Args)
                {
                    try
                    {
                        String[] addressParts = srvip.Split(':');
                        Server srv = new Server(IPAddress.Parse(addressParts[0]), Int32.Parse(addressParts[1]));
                        servers.Add(srv);
                    }
                    catch (FormatException) { }
                }
            MainWindow wnd = new MainWindow(servers.ToArray());
            wnd.Show();
        }
    }
}
