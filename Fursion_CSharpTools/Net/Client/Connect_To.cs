using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Fursion_CSharpTools;
using System.IO;

namespace Fursion_CSharpTools.Net.Client
{
    public class Connect_To
    {
        Socket socket;
        const int BUFFERS_SIZE = 1024;
        public bool State = false;
        public byte[] Buffers = new byte[BUFFERS_SIZE];
        public int buffCount = 0;
        public int MessageLenght = 0;
        public MemoryStream BufferStream;
        public ConnnctAction Action;
        public void Connect(string HOST, int PORT)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress iP = IPAddress.Parse(HOST);
            try
            {
                socket.Connect(iP, PORT);
                socket.BeginReceive(Buffers, 0, BUFFERS_SIZE, SocketFlags.None, AsyncReceiveCb, null);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }   
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

        }
        private void AsyncReceiveCb(IAsyncResult Ar)
        {
            try
            {
                int count = socket.EndReceive(Ar);
                Console.WriteLine("接收到" + count + ":{0}个字节的数据", count);
                if (count < 0)
                {
                    //有待修改
                    Close();
                }
                buffCount += count;
                PossingData();
            }
            catch (Exception e)
            {
                Console.WriteLine(" :连接异常 已经断开");
                Console.WriteLine(e.Message + "  From:  ServerMain.AsyncReceiveCb");
                Close();
            }
        }
        private void PossingData()
        {
            if (buffCount < sizeof(Int32))
            {
                socket.BeginReceive(Buffers, buffCount, BuffRemain(), SocketFlags.None, AsyncReceiveCb, this);
            }
            else
            {
                Int32 MessageLen = BitConverter.ToInt32(Buffers, 0);//取包头获取单个数据包长度
                if (buffCount < sizeof(Int32) + MessageLen && buffCount < BUFFERS_SIZE)
                {
                    socket.BeginReceive(Buffers, buffCount, BuffRemain(), SocketFlags.None, AsyncReceiveCb, this);
                }
                else if (buffCount < sizeof(Int32) + MessageLen && BUFFERS_SIZE < sizeof(Int32) + MessageLen)
                {
                    try
                    {
                        MemoryStream stream = new MemoryStream();
                        stream.Write(Buffers, sizeof(Int32), buffCount - sizeof(Int32));
                        MessageLenght = MessageLen;
                        BufferStream = stream;
                        RestBuffCount();
                        socket.BeginReceive(Buffers, buffCount, StreamBuffLen(), SocketFlags.None, SubReceiveCb, this);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "  ：分包接收");
                    }
                }
                else
                {
                    try
                    {
                        byte[] data = new byte[MessageLen];
                        Array.Copy(Buffers, sizeof(Int32), data, 0, MessageLen);
                        DataTransfer(data);
                        buffCount -= (MessageLen + sizeof(Int32));
                        if (buffCount > 0)
                        {
                            Array.Copy(Buffers, sizeof(Int32) + MessageLen, Buffers, 0, buffCount);
                            PossingData();
                        }
                        else
                        {
                            socket.BeginReceive(Buffers, buffCount, BuffRemain(), SocketFlags.None, AsyncReceiveCb, this);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + " pd");
                    }
                }
            }
        }
        /// <summary>
        /// 增量接收
        /// </summary>
        /// <param name="ar"></param>
        private void SubReceiveCb(IAsyncResult ar)
        {
            try
            {
                int count = socket.EndReceive(ar);
                Console.WriteLine("收到{0}个byte数据", count);
                buffCount = count;
                if (count < 0)
                {
                    Close();
                }
                else if (count == 0)
                {
                    MStreamDispose();
                    socket.BeginReceive(Buffers, buffCount, BuffRemain(), SocketFlags.None, AsyncReceiveCb, this);
                }
                else
                {
                    BufferStream.Write(Buffers, 0, buffCount);
                    if (MessageLenght == BufferStream.Length)
                    {
                        var bs = BufferStream.ToArray();
                        DataTransfer(bs);
                        MStreamDispose();
                        RestBuffCount();
                        socket.BeginReceive(Buffers, buffCount, BuffRemain(), SocketFlags.None, AsyncReceiveCb, this);
                    }
                    else if (MessageLenght > BufferStream.Length)
                    {
                        socket.BeginReceive(Buffers, buffCount, StreamBuffLen(), SocketFlags.None, SubReceiveCb, this);
                    }
                    else
                    {
                        Console.WriteLine("MS < s.lenght");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "Sub err");
            }

        }
        private void DataTransfer(byte[] bs)
        {
            bs.PrintByteArray();
        }
        public void Send(byte[] bs)
        {
            try
            {
                socket.Send(bs);
            }
            catch
            {

            }
        }
        public void Close()
        {
            socket.Close();
            socket.Dispose();
        }
    }
}
