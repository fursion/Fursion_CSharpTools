using System;
using System.Collections.Generic;
using System.Text;
// 调试类工具集
namespace Fursion_CSharpTools.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public static class FDebug
    {
        public static void Log(string o,params object [] arg)
        {
            LogHeader();
            Console.WriteLine(o,arg);
        }
        public static void Log(Object O)
        {
            LogHeader();
            Console.WriteLine(O);
        }
        /// <summary>
        /// 输出日期和时间
        /// </summary>
        private static void LogHeader()
        {
            Console.Write("["+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"]   ") ;
        }
    }
}
