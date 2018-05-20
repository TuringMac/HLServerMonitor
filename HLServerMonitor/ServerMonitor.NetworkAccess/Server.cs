using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ServerMonitor.Lib.Data;

namespace ServerMonitor.NetworkAccess
{
    public class Server : IServer, INotifyPropertyChanged
    {
        ServerState _state = ServerState.Unknown;
        String _name = null;
        String _map = null;
        int _players = -1;
        int _maxplayers = -1;

        public Server(IPAddress pAddress, int pPort)
        {
            Address = pAddress;
            Port = pPort;
            State = ServerState.Unknown;
        }

        #region Члены IServer

        public ServerState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Map
        {
            get
            {
                return _map;
            }
            set
            {
                if (_map != value)
                {
                    _map = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Players
        {
            get
            {
                return _players;
            }
            set
            {
                if (_players != value)
                {
                    _players = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int MaxPlayers
        {
            get
            {
                return _maxplayers;
            }
            set
            {
                if (_maxplayers != value)
                {
                    _maxplayers = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void Clear()
        {
            State = ServerState.Unknown;
            Name = "-";
            Map = "-";
            Players = -1;
            MaxPlayers = 0;
        }

        public IPAddress Address
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }

        #endregion

        #region Члены INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
