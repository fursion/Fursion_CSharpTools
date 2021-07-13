using System;

namespace DiskLogEdit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "LogEdit";
            string explain = "                  说明：使用时将保存log文件的文件夹拖入次终端框程序将自动在文件夹的上级目录生成PCLogOut文件夹来保存处理过后的log文件";
            Console.WriteLine(explain);
            Console.WriteLine("输入日志文件保存路径！： 拖拽文件夹");
            string folderpath = Console.ReadLine();
            ReadFile.find_logs(folderpath);
            while (true)
            {
                
            }
        }
    }
}
