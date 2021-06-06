using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
// 调试类工具集
namespace Fursion_CSharpTools.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class FDebug : Singleton<FDebug>, IDisposable
    {
        bool disposed = false;
        TextWriter TextWriter;
        public FDebug()
        {
            Console.WriteLine("log ");
            StreamWriter stream = File.AppendText(@"F:\GameServer\GameServerMain\Logs\log.txt");
            TextWriter = stream;
        }
        public static void Log(string logMessage, params object[] arg)
        { 
            LogHeader();
            Console.WriteLine(logMessage, arg);
            using(StreamWriter stream = File.AppendText(@"F:\GameServer\GameServerMain\Logs\log.txt"))
            {
                Log(logMessage, stream, arg);
            }
            
        }
        /// <summary>
        /// 输出日期和时间
        /// </summary>
        private static void LogHeader()
        {
            Console.Write("[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "]   ");
        }
        public static void Log(string logMessage, TextWriter w, params object[] arg)
        {
            LogHeader(w);
            //w.WriteLineAsync(logMessage, arg);
            w.WriteLine(logMessage, arg);
        }
        private static void LogHeader(TextWriter w)
        {
            w.WriteAsync("[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "]   ");
            //w.Write("[" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "]   ");
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                TextWriter.Dispose();
            }
            disposed = true;
        }
    }
}
