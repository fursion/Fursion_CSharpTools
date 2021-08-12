using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using Fursion_CSharpTools.Core;
using System.Threading;
using System.Threading.Tasks;
using Fursion_CSharpTools.AsyncJob;

namespace Fursion_CSharpTools.Net.UDP
{
    public class UDP_Data_Receive_Component : BaseComponent
    {
        public bool IsUse { get; set; }
        public const int BUFFER_SIZE = 1024;
        public byte[] Buffer { get; set; }
        public UdpClient UDP_Server;
        public Socket R_Socket { get; set; }
        public IPEndPoint EndPoint;
        public UDP_Data_Receive_Component()
        {
            Buffer = new byte[BUFFER_SIZE];
            EndPoint = new IPEndPoint(IPAddress.Any, 0);
        }
        public UDP_Data_Receive_Component(UdpClient udpClient)
        {
            Buffer = new byte[BUFFER_SIZE];
            UDP_Server = udpClient;
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
        private UdpClient UDP_Service;
        private Socket UDP_Service_Socket;
        private const int BUFFER_SIZE = 1024;
        private byte[] ReadBuffer;
        int i = 0;
        private FileStream Save_stream;
        //private IPEndPoint IPEndPoint_Receive;
        public void SocketInit(string IpAddress, int Port)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(IpAddress), Port);
            UDP_Service = new UdpClient(ipe);
            Save_stream = new FileStream(@"D:\迅雷下载\Savefile.txt", FileMode.Create, FileAccess.Write);
            ThreadPool.QueueUserWorkItem(UDP_Receive);

        }
        EndPoint TempRemoteEp = new IPEndPoint(IPAddress.Any, 0);
        public void UDP_Receive(object State)
        {
            var udpd = UDPComponentPool.UDPDataComponent(UDP_Service);
            UDP_Service.BeginReceive(BeginReceiveCallback, udpd);
        }
        public void BeginReceiveCallback(IAsyncResult ar)
        {
            i++;
            UDP_Data_Receive_Component uDP_Data_ = (UDP_Data_Receive_Component)ar.AsyncState;
            var dd = uDP_Data_.UDP_Server.EndReceive(ar, ref uDP_Data_.EndPoint);
            Console.WriteLine(i);
            var udpd = UDPComponentPool.UDPDataComponent(UDP_Service);
            UDP_Service.BeginReceive(BeginReceiveCallback, uDP_Data_);
            UDPtask uDPtask = new UDPtask { stream=Save_stream, UDP_Data_ = dd };
            TaskCore.Run(uDPtask);
        }
    }
    public struct UDPtask : IJobTask
    {
        public FileStream stream;
        public byte[]  UDP_Data_;
        public void CallBack(object obj)
        {
            throw new NotImplementedException();
        }

        public void Execute(object obj)
        {
            stream.Write(UDP_Data_);
        }
    }
}
