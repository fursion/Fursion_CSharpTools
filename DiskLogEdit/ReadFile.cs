using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Fursion_CSharpTools.Tools;

namespace DiskLogEdit
{
    public class ReadFile
    {
        public static void find_logs(string folderPath)
        {
            var files = F_IO.SearchDirectory(@folderPath, "*.log");
            var pathinfo = new DirectoryInfo(@folderPath);
            var newpath = pathinfo.Parent.CreateSubdirectory("PCLogOut");
            newpath.Refresh();
            Console.WriteLine(newpath.FullName);
            foreach(var item in files)
            {
                Editfile.Editlog(item, newpath.FullName);
            }
            
        }


    }
}
