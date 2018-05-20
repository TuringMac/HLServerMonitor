using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerMonitor.Lib.Data;
using ServerMonitor.Lib.Network;

namespace ServerMonitor.NetworkAccess
{
    class Watcher : IWatcher
    {
        byte[] infoRequest = { 0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00 };
        byte[] packHeader = { 0xFF, 0xFF, 0xFF, 0xFF };
        byte[] infoHeader = { 0x49 };

        readonly int RecieveDelay = 100;

        bool Fill(IServer pServer, byte[] pData = null)
        {
            if (!ValidateHeader(packHeader, pData))
                return false;

            pData = pData.Skip(packHeader.Length).ToArray();
            if (!ValidateHeader(infoHeader, pData))
                return false;

            pData = pData.Skip(infoHeader.Length).ToArray();

            byte protocol = pData[0];

            pData = pData.Skip(1).ToArray();
            String name = ReadString(pData);

            pData = pData.Skip(name.Length + 1).ToArray();
            String map = ReadString(pData);

            pData = pData.Skip(map.Length + 1).ToArray();
            String folder = ReadString(pData);

            pData = pData.Skip(folder.Length + 1).ToArray();
            String game = ReadString(pData);

            pData = pData.Skip(game.Length + 1).ToArray();
            short id = (short)((pData[0] << 8) & pData[1]);

            pData = pData.Skip(sizeof(short)).ToArray();
            byte players = pData[0];

            pData = pData.Skip(1).ToArray();
            byte maxplayers = pData[0];

            pData = pData.Skip(1).ToArray();

            pServer.State = ServerState.Online;
            pServer.Name = name;
            pServer.Map = map;
            pServer.Players = players;
            pServer.MaxPlayers = maxplayers;

            return true;
        }

        bool ValidateHeader(byte[] pHeader, byte[] pData)
        {
            if (pData.Length >= pHeader.Length)
            {
                for (int i = 0; i < pHeader.Length; i++)
                {
                    if (pData[i] != pHeader[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
                return false;
        }

        String ReadString(byte[] pData)
        {
            String result = "";
            int i = 0;
            while (pData[i] != 0x00)
            {
                result += Convert.ToChar(pData[i]);
                i++;
            }
            return result;
        }

        static Socket GetSocket()
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 3000); // Timeout after 3 seconds

            return s;
        }

        #region Члены IWatcher

        public bool Update(IServer pServer)
        {
            //TODO Add processing marker Red or green led, or turning ring
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.RunWorkerAsync(pServer);
            return true;
            /*
            Socket socket = GetSocket();
            bool result = false;
            int nBytesSent = 0;
            nBytesSent = socket.SendTo(infoRequest, new IPEndPoint(pServer.Address, pServer.Port));

            Thread.Sleep(RecieveDelay);

            int bufferSize = 1404; //valve pack size
            byte[] buff = new byte[bufferSize];
            EndPoint ep = new IPEndPoint(IPAddress.Any, 0) as EndPoint;

            int nBytes;
            try
            {
                nBytes = socket.ReceiveFrom(buff, ref ep);
            }
            catch (SocketException ex)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                pServer.Clear();
                return false;
            }
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();


            if (nBytes > 0)
            {
                if (!Fill(pServer, buff.Take(nBytes).ToArray()))
                    pServer.Clear();
            }
            else
                result = false;


            return true;
            */
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            (sender as BackgroundWorker).Dispose();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
#if DEBUG
            Thread.Sleep(8000);
#endif
            IServer pServer = e.Argument as IServer;
            Socket socket = GetSocket();
            bool result = false;
            int nBytesSent = 0;
            nBytesSent = socket.SendTo(infoRequest, new IPEndPoint(pServer.Address, pServer.Port));

            Thread.Sleep(RecieveDelay);

            int bufferSize = 1404; //valve pack size
            byte[] buff = new byte[bufferSize];
            EndPoint ep = new IPEndPoint(IPAddress.Any, 0) as EndPoint;

            int nBytes = 0;
            try
            {
                nBytes = socket.ReceiveFrom(buff, ref ep);
            }
            catch (SocketException ex)
            {
                Debug.WriteLine("SOCKET: " + ex.Message);
                //socket.Shutdown(SocketShutdown.Both);
                //socket.Close();
                pServer.Clear();
                pServer.State = ServerState.Offline;
                //return;
            }
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            if (nBytes > 0)
            {
                if (!Fill(pServer, buff.Take(nBytes).ToArray()))
                    pServer.Clear();
            }
            else
                result = false;

            return;
        }

        #endregion
    }
}
