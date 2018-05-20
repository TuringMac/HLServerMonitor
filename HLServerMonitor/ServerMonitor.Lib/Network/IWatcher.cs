using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerMonitor.Lib.Data;

namespace ServerMonitor.Lib.Network
{
    public interface IWatcher
    {
        bool Update(IServer pServer);
    }
}
