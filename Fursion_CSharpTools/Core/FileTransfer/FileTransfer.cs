using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using Fursion_CSharpTools.Net;

namespace Fursion_CSharpTools.Core.FileTransfer
{
    public enum FileTransferState
    {
        None,
        Preparation,
        Transmitting,
        TransmissionComplete,
        Cancelled
    }

    public enum FileTransferType
    {
        Server,
        Client
    }
    /*
     * FILE_NUMBER
     * CHUNK_NUMBER
     * DATA
     */
    /// <summary>
    /// 提供文件传输功能的服务
    /// </summary>

    public class FileTransfer : TransferService, IDisposable
    {
        private bool CancelToken = false;
        private FileTransferState transferState;
        public FileTransferState FileTransferState { get { return transferState; } }
        public TCP_Only_Service TCP_Only_Service { get; set; }//TCP监听服务
        public const int FILE_CHUNK = 1024 * 1024 * 1;//每个文件块的最大大小
        public string CURL;
        private TcpClient Client;
        /// <summary>
        /// 文件切片
        /// </summary>
        public void FileSlicing()
        {
            //findfile(path);
        }
        public void findfile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                CancelToken = true;
                CloseService();
                //关闭服务
            }
            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        }
        private void Getfileinfo(string filepath)
        {
            FileInfo fileInfo = new FileInfo(filepath);
            Console.WriteLine("准备发送文件 {0} === 总计{1} byte",fileInfo.Name,fileInfo.Length);
        }
        private void Sendfileinfo()
        {

        }
        public void StartTransfer()
        {

        }
        public void Sendfile()
        {

        }
        private void CloseService()
        {
            if(CancelToken)
                Console.WriteLine("关闭");
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
