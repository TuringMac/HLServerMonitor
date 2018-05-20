using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitor.Lib.Data
{
    public enum ServerState
    {
        [Description("Неизвествно")]
        Unknown = 1,
        [Description("Отключен")]
        Offline = 2,
        [Description("В сети")]
        Online = 4,
        [Description("Опрашиваю")]
        Updating = 8
    }
    public interface IServer
    {
        ServerState State { get; set; }
        String Name { get; set; }
        String Map { get; set; }
        int Players { get; set; }
        int MaxPlayers { get; set; }
        void Clear();

        IPAddress Address { get; }
        int Port { get; }
    }
}
