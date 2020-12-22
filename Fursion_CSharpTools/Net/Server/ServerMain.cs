using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Fursion_CSharpTools.Net.Public;
using Fursion_CSharpTools;
using Protols;
using ProtocolTools;

namespace Fursion_CSharpTools.Net.Server
{
    /// <summary>
    /// Socket服务器端监听类
    /// </summary>
    public class ServerMain : Singleton<ServerMain>
    {
        Socket Server_Socket;
        SocketCallBack SocketCall;
        public Connect[] connects;
        const int MAX_CONNECT_NUMBERS = 5000;
        public int StarServer()
        {
            Server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connects = new Connect[MAX_CONNECT_NUMBERS];
            IPEndPoint iPEnd = new IPEndPoint(IPAddress.Any, 1024);
            Server_Socket.Bind(iPEnd);
            Server_Socket.Listen(10);
            Server_Socket.BeginAccept(AsyncAccept, null);
            Console.WriteLine(iPEnd.Address.ToString());
            return 1;
        }
        public void StarServer(string IP, int Port, SocketCallBack callBack)
        {

            Server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connects = new Connect[MAX_CONNECT_NUMBERS];
            IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), Port);
            if (callBack != null)
                SocketCall = callBack;
            Server_Socket.Bind(iPEnd);
            Server_Socket.Listen(10);
            Server_Socket.BeginAccept(AsyncAccept, null);
            Console.WriteLine(iPEnd.Address.ToString());
        }
        public int Get_Connect_Index()
        {
            if (connects == null)
                return -1;
            for (int i = 0; i < MAX_CONNECT_NUMBERS; i++)
            {
                if (connects[i] == null)
                {
                    connects[i] = new Connect();
                    return i;
                }
                else if (!connects[i].State_IsUSE)
                {
                    return i;
                }
            }
            return -1;
        }
        public void AsyncAccept(IAsyncResult Ar)
        {
            try
            {
                Socket socket = Server_Socket.EndAccept(Ar);
                int Index = Get_Connect_Index();
                if (Index < 0)
                {
                    socket.Close();
                    Console.WriteLine("警告：连接已满");
                }
                else
                {
                    Connect connect = connects[Index];
                    connect.Init(socket);
                    Console.WriteLine("与{0}建立连接",connect.GetAddress());
                    connect.Connect_Socket.BeginReceive(connect.Buffers, connect.buffCount, connect.BuffRemain(), SocketFlags.None, AsyncReceiveCb, connect);
                }
                Server_Socket.BeginAccept(AsyncAccept, null);

            }
            catch (Exception e)
            {
                Console.WriteLine("AsyncAccept失败 ：" + e.Message);
            }

        }
        private void AsyncReceiveCb(IAsyncResult Ar)
        {
            Connect connect = (Connect)Ar.AsyncState;
            try
            {
                int count = connect.Connect_Socket.EndReceive(Ar);
                if (count < 0)
                {
                    //有待修改
                    connect.Close();
                }
                connect.buffCount += count;
                PossingData(connect);               
            }
            catch (Exception e)
            {
                Console.WriteLine(connect.GetAddress() + " :连接异常 已经断开");
                Console.WriteLine(e.Message + "  From:  ServerMain.AsyncReceiveCb");
                connect.Close();
            }
        }
        private void PossingData(Connect connect)
        {
            if (connect.buffCount < sizeof(Int32))
            {
                connect.Connect_Socket.BeginReceive(connect.Buffers, connect.buffCount, connect.BuffRemain(), SocketFlags.None, AsyncReceiveCb, connect);
            }
            else
            {
                Int32 MessageLen = BitConverter.ToInt32(connect.Buffers, 0);//取包头获取单个数据包长度
                if (connect.buffCount < sizeof(Int32) + MessageLen && connect.buffCount < Connect.BUFFERS_SIZE)
                {
                    connect.Connect_Socket.BeginReceive(connect.Buffers, connect.buffCount, connect.BuffRemain(), SocketFlags.None, AsyncReceiveCb, connect);
                }
                else if (connect.buffCount < sizeof(Int32) + MessageLen && Connect.BUFFERS_SIZE < sizeof(Int32) + MessageLen)
                {
                    try
                    {
                        MemoryStream stream = new MemoryStream();
                        stream.Write(connect.Buffers, sizeof(Int32), connect.buffCount - sizeof(Int32));
                        connect.MessageLenght = MessageLen;
                        connect.BufferStream = stream;
                        connect.RestBuffCount();
                        connect.Connect_Socket.BeginReceive(connect.Buffers, connect.buffCount, connect.StreamBuffLen(), SocketFlags.None, SubReceiveCb, connect);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message+"  ：分包接收");
                    }
                }
                else
                {
                    try
                    {
                        byte[] data = new byte[MessageLen];
                        Array.Copy(connect.Buffers, sizeof(Int32), data, 0, MessageLen);
                        DataTransfer(data,connect);
                        connect.buffCount -= (MessageLen + sizeof(Int32));
                        if (connect.buffCount > 0)
                        {
                            Array.Copy(connect.Buffers, sizeof(Int32) + MessageLen, connect.Buffers, 0, connect.buffCount);
                            PossingData(connect);
                        }
                        else
                        {
                            connect.Connect_Socket.BeginReceive(connect.Buffers, connect.buffCount, connect.BuffRemain(), SocketFlags.None, AsyncReceiveCb, connect);
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
            Connect connect = (Connect)ar.AsyncState;
            try
            {
                int count = connect.Connect_Socket.EndReceive(ar);
                connect.buffCount = count;
                if (count < 0)
                {
                    connect.Close();
                }
                else if (count == 0)
                {
                    connect.MStreamDispose();
                    connect.Connect_Socket.BeginReceive(connect.Buffers, connect.buffCount, connect.BuffRemain(), SocketFlags.None, AsyncReceiveCb, connect);
                }
                else
                {
                    connect.BufferStream.Write(connect.Buffers, 0, connect.buffCount);
                    if (connect.MessageLenght == connect.BufferStream.Length)
                    {
                        var bs = connect.BufferStream.ToArray();
                        DataTransfer(bs,connect);
                        connect.MStreamDispose();
                        connect.RestBuffCount();
                        connect.Connect_Socket.BeginReceive(connect.Buffers, connect.buffCount, connect.BuffRemain(), SocketFlags.None, AsyncReceiveCb, connect);
                    }
                    else if (connect.MessageLenght > connect.BufferStream.Length)
                    {
                        connect.RestBuffCount();
                        connect.Connect_Socket.BeginReceive(connect.Buffers, connect.buffCount, connect.StreamBuffLen(), SocketFlags.None, SubReceiveCb, connect);
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
        /// <summary>
        /// 数据传送
        /// </summary>
        /// <param name="bs"></param>
        private void DataTransfer(byte[] bs,Connect connect)
        {
            Console.WriteLine("接收完成共{0}byte数据 From:{1}",bs.Length,connect.GetAddress());
            IPC.GetInstance().InComing_DATA(bs,connect);
           
        }
    }
}
