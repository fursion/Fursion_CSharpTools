using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Fursion_CSharpTools.Net
{
    public abstract class TCP_Only_Service : IDisposable
    {
        public bool State;
        public void Dispose()
        {
            if (!State)
            {
                TcpServerListener.Stop();
                GC.SuppressFinalize(this);
            }
               
        }
        public TcpListener TcpServerListener;
        public TcpClient TcpClient;
        public TCP_Only_Service(string ipaddress, int Port)
        {
            IPAddress iPAddress = IPAddress.Parse(ipaddress);
            TcpServerListener = new TcpListener(iPAddress, Port);
            TcpServerListener.Start(1);
            DoBeginAcceptTcpClient(TcpServerListener);
        }
        public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
        public  void DoBeginAcceptTcpClient(TcpListener listener)
        {
            tcpClientConnected.Reset();
            listener.BeginAcceptTcpClient(DoAcceptTcpClientCallback,this);
            tcpClientConnected.WaitOne();
        }
        public  void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient = listener.EndAcceptTcpClient(ar);
            tcpClientConnected.Set();
            
        }
    }
}
