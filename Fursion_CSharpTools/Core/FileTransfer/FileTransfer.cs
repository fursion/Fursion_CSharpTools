using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools.Core.FileTransfer
{
    /*
     * FILE_NUMBER
     * CHUNK_NUMBER
     * DATA
     */
    /// <summary>
    /// 提供文件传输功能的类
    /// </summary>
    public class FileTransfer:Service
    {
        public const int FILE_CHUNK = 1024 * 1024 * 1;//每个文件块的最大大小
        /// <summary>
        /// 文件切片
        /// </summary>
        public static void FileSlicing()
        {
            long filesize;
            int ChunkCount;
            string path = @"D:\迅雷下载\洛基.Loki.S01E02.1080p.60fps.H265-NEW字幕组.mp4.bt.xltd";
            FileStream fileStream;
            fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            Console.WriteLine(fileStream.Length);
        }
    }
}
