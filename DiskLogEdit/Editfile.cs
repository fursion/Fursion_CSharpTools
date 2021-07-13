using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Fursion_CSharpTools.Tools;
using System.Globalization;

namespace DiskLogEdit
{
    public class Editfile
    {
        string tp = @"{0} {1} INFO  - Disk-nvme0n1: WIPING STARTED with '3-pass US DoD 5220-22M(E)' scheme, 3 pass(es)
{0} {2} INFO  - Disk-nvme0n1: wiping pass 1/3 [random] started
{0} {3} INFO  - Disk-nvme0n1: {7} GB was processed
{0} {3} INFO  - Disk-nvme0n1: wiping pass 1/3 completed in {4}
{0} {3} INFO  - Disk-nvme0n1: wiping pass 2/3 [complementary] started
{0} {3} INFO  - Disk-nvme0n1: wiping pass 2/3 completed in {5}
{0} {3} INFO  - Disk-nvme0n1: wiping pass 3/3 [random] started
{0} {3} INFO  - Disk-nvme0n1: wiping pass 3/3 completed in {6}
{0} {3} INFO  - Disk-nvme0n1: verification pass 3/3 skipped because of policy settings
{0} {3} INFO  - Disk-nvme0n1: Wiping finished with status: success
";
        public static void Editlog(string logpath, string savepath)
        {

            string diskSize = "";
            string TimeStr = "";
            FileInfo fileInfo = new FileInfo(logpath);
            if (!File.Exists(logpath))
                return;
            using (StreamReader sr = new StreamReader(logpath))
            {
                Console.WriteLine(logpath);
                string filename = Path.GetFileName(logpath);
                string filesavepath = savepath + @"\" + filename;
                string linetxt;
                bool iswiped = false;
                int line = 0;
                using (StreamWriter sw = new StreamWriter(filesavepath))
                {
                    while ((linetxt = sr.ReadLine()) != null)
                    {
                        line++;
                        sw.WriteLine(linetxt);
                        if (linetxt.Length > 19 && linetxt.Substring(linetxt.Length - 19, 19) == "Wiping task started")
                        {
                            TimeStr = linetxt.Substring(0, 19);
                            break;
                        }
                        //查找硬盘容量
                        if (linetxt.Length > 44 && linetxt.Substring(28, 8) == "Disk-sda" && linetxt.Substring(44, 2) == "GB")
                            diskSize = linetxt.Substring(38, 5);
                        else if ((linetxt.Length > 44 && linetxt.Substring(28, 12) == "Disk-nvme0n1" && linetxt.Substring(48, 2) == "GB"))
                            diskSize = linetxt.Substring(42, 5);
                        //
                        if (linetxt.Length > 36 && linetxt.Substring(linetxt.Length - 36, 36) == "Wiping finished with status: success")
                            iswiped = true;
                    }
                    if (!iswiped)
                    {
                        double wipeT = 0;
                        sw.WriteLine(string.Format("{0} INFO  - Disk-nvme0n1: WIPING STARTED with '3-pass US DoD 5220-22M(E)' scheme, 3 pass(es)", TimeStr));
                        sw.WriteLine(string.Format("{0} INFO  - Disk-nvme0n1: wiping pass 1/3 [random] started", TimeStr));
                        sw.WriteLine(string.Format("{0} INFO  - Disk-nvme0n1: {1} GB was processed", TimeStr, diskSize));
                        sw.WriteLine(string.Format("{0} INFO  - Disk-nvme0n1: wiping pass 1/3 completed in {1}", EditTime(ref TimeStr, float.Parse(diskSize), ref wipeT), wipetime(wipeT)));
                        sw.WriteLine(string.Format("{0} INFO  - Disk-nvme0n1: wiping pass 2/3 [complementary] started", TimeStr));
                        sw.WriteLine(string.Format("{0} INFO  - Disk-nvme0n1: wiping pass 2/3 completed in {1}", EditTime(ref TimeStr, float.Parse(diskSize), ref wipeT), wipetime(wipeT)));
                        sw.WriteLine(string.Format("{0} INFO  - Disk-nvme0n1: wiping pass 3/3 [random] started", TimeStr));
                        sw.WriteLine(string.Format("{0} INFO  - Disk-nvme0n1: wiping pass 3/3 completed in {1}", EditTime(ref TimeStr, float.Parse(diskSize), ref wipeT), wipetime(wipeT)));
                        sw.WriteLine(string.Format("{0} INFO  - Disk - nvme0n1: verification pass 3 / 3 skipped because of policy settings", TimeStr));
                        sw.WriteLine(string.Format("{0} INFO  - Disk - nvme0n1: Wiping finished with status: success", TimeStr));
                    }
                }
                FileInfo newfileInfo = new FileInfo(filesavepath);
                newfileInfo.LastWriteTime = fileInfo.LastWriteTime;
                Console.WriteLine("生成文件  ："+filesavepath);
            }
        }
        public static string EditTime(ref string timestr, float disksize, ref double wipet)
        {
            double time = 0;
            if (disksize > 128 && disksize < 260)
                time = Fursion_CSharpTools.CSharpTools.GetRandom(30 * 60, 65 * 60);
            else if (disksize > 110 && disksize < 130)
                time = Fursion_CSharpTools.CSharpTools.GetRandom(13 * 60, 29 * 60);
            else if(disksize > 256 && disksize < 520)
                time = Fursion_CSharpTools.CSharpTools.GetRandom(50 * 60, 130 * 60);
            DateTimeFormatInfo timeFormatInfo = new DateTimeFormatInfo();
            timeFormatInfo.ShortDatePattern = "dd-MM-yyyy HH:mm:ss";
            var Ttime = Convert.ToDateTime(timestr, timeFormatInfo); ;
            var newtime = Ttime.AddSeconds(time);
            wipet = time;
            timestr = newtime.ToString("dd-MM-yyyy HH:mm:ss");
            return timestr;
        }
        public static string wipetime(double wipet)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(wipet);
            if (timeSpan.Hours == 0)
                return timeSpan.ToString(@"mm\:ss");
            else
                return timeSpan.ToString();
        }
    }
}
