using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerMonitor.Lib.Data;

namespace ServerMonitor.WPF
{
    class ServerDetailsViewModel
    {
        IServer _server;

        public String Address
        {
            get
            {
                if (_server == null)
                    return "";
                return _server.Address.ToString();
            }
        }

        public String Name
        {
            get
            {
                if (_server == null)
                    return "";
                return _server.Name;
            }
        }

        public String Map
        {
            get
            {
                if (_server == null)
                    return "";
                return _server.Map;
            }
        }

        public int Players
        {
            get
            {
                if (_server == null)
                    return 0;
                return _server.Players;
            }
        }

        public int MaxPlayers
        {
            get
            {
                if (_server == null)
                    return 0;
                return _server.MaxPlayers;
            }
        }
    }
}
