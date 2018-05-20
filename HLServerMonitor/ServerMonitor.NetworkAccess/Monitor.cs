using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerMonitor.Lib;
using ServerMonitor.Lib.Data;
using ServerMonitor.Lib.Network;

namespace ServerMonitor.NetworkAccess
{
    public class Monitor : IMonitor
    {
        public Monitor()
        {
            ServerList = new List<IServer>();
        }

        #region Члены IMonitor

        public List<IServer> ServerList
        {
            get;
            private set;
        }

        public void Add(IServer pServer)
        {
            if (pServer.Address != null && pServer.Port != 0)
                ServerList.Add(pServer);
        }

        public void Remove(IServer pServer)
        {
            ServerList.Remove(pServer);
        }

        public void Refresh()
        {
            IWatcher watcher = new Watcher();
            foreach (IServer srv in ServerList)
            {
                srv.State = ServerState.Updating;
                watcher.Update(srv);
            }
        }

        #endregion
    }
}
