using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerMonitor.Lib.Data;

namespace ServerMonitor.Lib
{
    public interface IMonitor
    {
        List<IServer> ServerList { get; }
        void Add(IServer pServer);
        void Remove(IServer pServer);
        void Refresh();
    }
}
