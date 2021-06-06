using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Fursion_CSharpTools.Tools
{

    public static class F_IO
    {
        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadTextfile(string path)
        {
            string t;
            if (File.Exists(path))
            {
                t = File.ReadAllText(path, Encoding.UTF8);
                return t;
            }
            else FDebug.Log("file {0} not found", path);
            return null;
        }
        /// <summary>
        /// 读取二进制文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] ReadBytefile(string path)
        {
            byte[] data;
            if (File.Exists(path))
            {
                data = File.ReadAllBytes(path);
                return data;
            }
            else FDebug.Log("file {0} not found", path);
            return null;

        }
        /// <summary>
        /// 创建文件并写入数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void CreateAndWrite(string path, byte[] data)
        {
            if (!File.Exists(path))
            {
                using (FileStream fs = File.Create(path))
                {
                    fs.Write(data, 0, data.Length);
                }
            }
        }       
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="TargetDirectory"></param>
        public static void CreateDirectory(string TargetDirectory)
        {
            DirectoryInfo di = new DirectoryInfo(TargetDirectory);
            try
            {
                if (di.Exists)
                {
                    FDebug.Log("That path exists already.");
                    return;
                }
                di.Create();
                FDebug.Log("The directory {0} was created successfully.",TargetDirectory);
            }
            catch (Exception e)
            {
                FDebug.Log("The process failed: {0}", e.ToString());
            }
            finally { }
        }
        /// <summary>
        /// 检索指定目录下给定类型的文件
        /// </summary>
        /// <param name="TargetDirectory">路径</param>
        /// <param name="searchPattern">文件扩展名 列 .xlsx  .docx * ?</param>
        /// <returns></returns>
        public static IEnumerable<string> SearchDirectory(string TargetDirectory,string searchPattern)
        {
            var files = Directory.EnumerateFiles(TargetDirectory, searchPattern, SearchOption.AllDirectories);
            foreach(var item in files)
            {
                FDebug.Log(item);
            }
            return files;
        }
    }
}
