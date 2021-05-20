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
            Console.WriteLine(o,arg);
        }
    }
}
