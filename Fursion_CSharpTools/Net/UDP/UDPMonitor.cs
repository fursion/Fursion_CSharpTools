using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using Fursion_CSharpTools.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Fursion_CSharpTools.Net.UDP
{
    public class UDP_Data_Receive_Component : BaseComponent
    {
        public bool IsUse { get; set; }
        public const int BUFFER_SIZE = 1024;
        public byte[] Buffer { get; set; }
        public Socket R_Socket { get; set; }
        public EndPoint EndPoint;
        public UDP_Data_Receive_Component()
        {
            Buffer = new byte[BUFFER_SIZE];
            EndPoint = new IPEndPoint(IPAddress.Any, 0);
        }
        public UDP_Data_Receive_Component(Socket socket)
        {
            Buffer = new byte[BUFFER_SIZE];
            R_Socket = socket;
            EndPoint = new IPEndPoint(IPAddress.Any, 0);
        }
        public override ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
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
        public int RestStart()
        {
            return 0;
        }
        ~UDPMonitor()
        {
            Dispose(false);
        }
        private Socket UDP_Service_Socket;
        private const int BUFFER_SIZE = 1024;
        private byte[] ReadBuffer;
        //private IPEndPoint IPEndPoint_Receive;
        public void SocketInit(string IpAddress, int Port)
        {
            UDP_Service_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint UDP_Service_IPAddress = new IPEndPoint(IPAddress.Parse(IpAddress), Port);
            UDP_Service_Socket.Bind(UDP_Service_IPAddress);

            ThreadPool.QueueUserWorkItem(UDP_Receive);

        }
        EndPoint TempRemoteEp = new IPEndPoint(IPAddress.Any, 0);
        public void UDP_Receive(object State)
        {
            while (true)
            {
                var udpd = UDPComponentPool.UDPDataComponent(UDP_Service_Socket);
                UDP_Service_Socket.BeginReceiveFrom(udpd.Buffer, 0, UDP_Data_Receive_Component.BUFFER_SIZE, SocketFlags.None, ref udpd.EndPoint, BeginReceiveCallback, udpd);
            }
        }
        public void BeginReceiveCallback(IAsyncResult ar)
        {
            UDP_Data_Receive_Component uDP_Data_ = (UDP_Data_Receive_Component)ar.AsyncState;
            int bytes = uDP_Data_.R_Socket.EndReceiveFrom(ar, ref uDP_Data_.EndPoint);

            var udpd = UDPComponentPool.UDPDataComponent(UDP_Service_Socket);
            UDP_Service_Socket.BeginReceiveFrom(udpd.Buffer, 0, UDP_Data_Receive_Component.BUFFER_SIZE, SocketFlags.None, ref udpd.EndPoint, BeginReceiveCallback, udpd);
        }
    }
}
