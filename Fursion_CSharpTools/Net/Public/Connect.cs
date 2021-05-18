using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Fursion_CSharpTools;
using ProtocolTools;
using System.IO;

namespace Fursion_CSharpTools.Net.Public
{
    public class Connect
    {
        public Socket Connect_Socket;
        public bool State_IsUSE = false;
        public const int BUFFERS_SIZE = 1024;
        public Byte[] Buffers;
        public int buffCount = 0;
        public int MessageLenght = 0;
        public MemoryStream BufferStream;
        public ConnnctAction Action;
        public void Init(Socket socket)
        {
            Buffers = new byte[BUFFERS_SIZE];
            if (socket == null)
                return;
            this.Connect_Socket = socket;
            State_IsUSE = true;
            buffCount = 0;
        }
        public int BuffRemain()
        {
            return BUFFERS_SIZE - buffCount;
        }
        public void RestBuffCount()
        {
            buffCount = 0;
        }
        public int StreamBuffLen()
        {
            if (BufferStream == null)
                return 0;
            int Len = MessageLenght - (int)BufferStream.Length;
            if (Len >= BUFFERS_SIZE)
                return BUFFERS_SIZE;
            else if (0 < Len && Len < BUFFERS_SIZE)
                return Len;
            else return 0;
        }
        public int MStreamDispose()
        {
            try
            {
                BufferStream.Close();
                BufferStream.Dispose();
                MessageLenght = 0;
                return 1;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
            
        }
        public void Send(byte[] bs)
        {
            var b = ProtocolTool.Packaging(bs);
            Connect_Socket.Send(b);
        }
        public string GetAddress()
        {
            if (!State_IsUSE)
                return "无法获取地址";
            return Connect_Socket.RemoteEndPoint.ToString();
        }
        public void Close()
        {
            if (!State_IsUSE)
                return;
            Console.WriteLine("断开连接：" + GetAddress());
            Connect_Socket.Close();
            State_IsUSE = false;
            
        }
    }
}
