using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using Fursion_CSharpTools.Tools;
using Fursion_CSharpTools.Net;
namespace Fursion_CSharpTools.Core.FileTransfer
{
    public class FileDownload :TransferService,IDisposable
    {
        public TcpClient TcpClient { get; set; }
        private NetworkStream DownNetworkStream;
        private const int ReadBufferSize = 1024;
        private byte[] ReadBuffer;
        private bool loaded = false;
        private bool IsDisposable = false;
        /// <summary>
        /// 表示文件是否下载完成的属性
        /// </summary>
        public bool Loaded { get { return loaded; } }
        public float progress = 0;
        /// <summary>
        /// 表示文件下载进度的属性
        /// </summary>
        public float Progress { get { return progress; } }
        public FileStream File_Stream { get; set; }

        public FileDownload()
        {


        }
        public void Start()
        {

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
            File_Stream.WriteAsync(ReadBuffer, 0, ReadBuffer.Length);
            UpdateProgress();
            while (networkStream.DataAvailable)
            {
                networkStream.BeginRead(ReadBuffer, 0, ReadBufferSize, BeginReadStreamCallback, networkStream);
            }

        }
        /// <summary>
        /// 更新进度
        /// </summary>
        private void UpdateProgress()
        {

        }
        public void CreatSaveFileStream()
        {
            //发起文件传输的请求,包含文件的属性信息 =>f ileinfo

            //检查存储路径是否存在
            //不存在则创建目录
            //新建临时文件保存数据

            //向临时文件写入数据
            //写入结束根据fileinfo修改文件信息
            //文件校验
            //更新文件下载结果
        }
        private bool CheckFile(string MD5)
        {

            return false;
        }
        public void Dispose()
        {
            if (IsDisposable)
                throw new NotImplementedException();
        }

        public void Disposeing()
        {
            TcpClient.Close();
            DownNetworkStream.Flush();
            DownNetworkStream.Close();
            File_Stream.Flush();
            File_Stream.Close();
            IsDisposable = true;
        }
    }
}
