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
using Fursion_CSharpTools.Tools;

namespace Fursion_CSharpTools.Net.Server
{
    /// <summary>
    /// Socket服务器端监听类
    /// </summary>
    public class TCPConnectMonitor : Singleton<TCPConnectMonitor>
    {
        Socket Server_Socket;
        /// <summary>
        /// 用以注册用户自定义函数
        /// </summary>
        public SocketCallBack SocketCall;
        /// <summary>
        /// 连接中间件数组
        /// </summary>
        public Connect[] connects;
        const int MAX_CONNECT_NUMBERS = 5000;
        /// <summary>
        /// 自动获取IP地址的启动函数
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public int StarServer(int Port,SocketCallBack callBack)
        {
            Server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connects = new Connect[MAX_CONNECT_NUMBERS];
            IPEndPoint iPEnd = new IPEndPoint(IPAddress.Any, Port);
            if (callBack != null)
                SocketCall += callBack;
            Server_Socket.Bind(iPEnd);
            Server_Socket.Listen(10);
            Server_Socket.BeginAccept(AsyncAccept, null);
            FDebug.Log(iPEnd.Address.ToString());
            return 1;
        }
        /// <summary>
        /// 自定义端口的启动函数
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        /// <param name="callBack"></param>
        public void StarServer(string IP, int Port, SocketCallBack callBack)
        {
            FDebug.Log("Service starting...");
            Server_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connects = new Connect[MAX_CONNECT_NUMBERS];
            IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse(IP), Port);
            if (callBack != null)
                SocketCall += callBack;
            Server_Socket.Bind(iPEnd);
            Server_Socket.Listen(10);
            Server_Socket.BeginAccept(AsyncAccept, null);
            FDebug.Log("ServiceAddressIp:{0}", iPEnd.Address.ToString());
            FDebug.Log("Service started Successfully!");
            
        }
        /// <summary>
        /// 获取一个空的连接类索引
        /// </summary>
        /// <returns>返回-1表示连接已满</returns>
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
                    FDebug.Log("警告：连接已满");
                }
                else
                {
                    Connect connect = connects[Index];
                    connect.Init(socket);
                    FDebug.Log("与{0}建立连接", connect.GetAddress());
                    connect.Connect_Socket.BeginReceive(connect.Buffers, connect.buffCount, connect.BuffRemain(), SocketFlags.None, AsyncReceiveCb, connect);
                }
                Server_Socket.BeginAccept(AsyncAccept, null);

            }
            catch (Exception e)
            {
                FDebug.Log("AsyncAccept失败 ：" + e.Message);
            }

        }
        /// <summary>
        /// 异步接收回调
        /// </summary>
        /// <param name="Ar"></param>
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
                FDebug.Log(connect.GetAddress() + " :连接异常 已经断开");
                FDebug.Log(e.Message + "  From:  ServerMain.AsyncReceiveCb");
                connect.Close();
            }
        }
        /// <summary>
        /// 对从网络缓冲区拿到的数据得处理函数
        /// </summary>
        /// <param name="connect">连接类的实例</param>
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
                        FDebug.Log(e.Message + "  ：分包接收");
                    }
                }
                else
                {
                    try
                    {
                        byte[] data = new byte[MessageLen];
                        Array.Copy(connect.Buffers, sizeof(Int32), data, 0, MessageLen);
                        DataTransfer(data, connect);
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
                        FDebug.Log(e.Message + " pd");
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
                        DataTransfer(bs, connect);
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
                        FDebug.Log("MS < s.lenght");
                    }
                }
            }
            catch (Exception e)
            {
                FDebug.Log(e.Message + "SubReceiveCb  err");
            }

        }
        /// <summary>
        /// 数据传送
        /// </summary>
        /// <param name="bs"></param>
        private void DataTransfer(byte[] bs, Connect connect)
        {
            ///
            ///待完善
            FDebug.Log("接收完成共{0}byte数据 From:{1}", bs.Length, connect.GetAddress());
            SocketCall?.Invoke(bs);
           // IPC.GetInstance().InComing_DATA(bs, connect);
            try
            {
                connect.Send(ProtocolTool.PackagingNetPackag(ProtocolBufType.RespondLogin, bs));
            }
            catch
            {
                FDebug.Log("发送失败");
            }
        }
    }
}
