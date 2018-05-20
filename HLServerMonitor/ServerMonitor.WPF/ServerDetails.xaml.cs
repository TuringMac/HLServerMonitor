using ServerMonitor.Lib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ServerMonitor.WPF
{
    /// <summary>
    /// Логика взаимодействия для ServerDetails.xaml
    /// </summary>
    public partial class ServerDetails : UserControl
    {
        IServer srv = null;
        public ServerDetails()
        {
            InitializeComponent();
            this.DataContextChanged += ServerDetails_DataContextChanged;
        }

        private void ServerDetails_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            srv = this.DataContext as IServer;
            if (srv == null) return;
            var srvu = srv as INotifyPropertyChanged;
            if (srvu == null) return;
            srvu.PropertyChanged += Srvu_PropertyChanged;
        }

        private void Srvu_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                switch (srv.State)
                {
                    case ServerState.Updating: grdSplash.Visibility = Visibility.Visible; break;
                    default:
                        grdSplash.Dispatcher.BeginInvoke(new Action(delegate ()
                        {
                            grdSplash.Visibility = Visibility.Hidden;
                        }));  break;
                }
            }
        }
    }
}
