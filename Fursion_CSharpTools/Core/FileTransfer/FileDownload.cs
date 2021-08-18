using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using Fursion_CSharpTools.Net;
namespace Fursion_CSharpTools.Core.FileTransfer
{
    public class FileDownload:IDisposable
    {
        public TcpClient TcpClient { get; set; }
        private NetworkStream DownNetworkStream;
        private const int ReadBufferSize = 1024;
        private byte[] ReadBuffer;
        public FileStream File_Stream { get; set; }

        public FileDownload()
        {

            //文件流实例化,先取随机名字再重命名
        }
        public void Connected(IAsyncResult ar)
        {
            TcpClient tcpClient = (TcpClient)ar.AsyncState;
            if (!tcpClient.Connected)
                return;
            DownNetworkStream = TcpClient.GetStream();
        }
        public void BeginReadStreamCallback(IAsyncResult ar)
        {
            NetworkStream networkStream = (NetworkStream)ar.AsyncState;
            int numberOfBytesRead;
            numberOfBytesRead = networkStream.EndRead(ar);
            File_Stream.BeginWrite(ReadBuffer, 0, ReadBufferSize, FileStreamBeginWriteCallback, File_Stream);
            while (networkStream.DataAvailable)
            {
                networkStream.BeginRead(ReadBuffer, 0, ReadBufferSize, BeginReadStreamCallback, networkStream);
            }

        }
        public void FileStreamBeginWriteCallback(IAsyncResult ar)
        {

        }
        public void BeginWriteStream(IAsyncResult ar)
        {

        }
        public void CreatFileStream()
        {
            //以时间民命
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        bool IsDisposable = false;
        public void Disposeing()
        {
            TcpClient.Close();
            DownNetworkStream.Flush();
            DownNetworkStream.Close();
            File_Stream.Flush();
            File_Stream.Close();
        }
    }
}
