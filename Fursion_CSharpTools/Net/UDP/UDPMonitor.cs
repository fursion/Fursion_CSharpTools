using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools.Net.UDP
{
    public class UDPMonitor : IDisposable
    {
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposeing)
        {
            if (disposed)
                return;
            if (disposeing)
            {

            }
            disposed = true;
        }
        ~UDPMonitor()
        {
            Dispose(false);
        }
        Socket UDP_Service_Socket;
        public  void SocketInit(string  IpAddress,int Port)
        {
            UDP_Service_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint UDP_Service_IPAddress = new IPEndPoint(IPAddress.Parse(IpAddress),Port);
            UDP_Service_Socket.Bind(UDP_Service_IPAddress);
        }
    }
}
