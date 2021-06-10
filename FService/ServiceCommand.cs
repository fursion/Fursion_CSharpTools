using System;
using System.Collections.Generic;
using System.Text;

using Fursion_CSharpTools;
using Fursion_CSharpTools.Net.Public;
using Fursion_CSharpTools.Tools;

namespace GameServerMain
{
    class ServiceCommand : Singleton<ServiceCommand>
    {
        public void CheckCommand()
        {
            string Commend = Console.ReadLine();
            switch (Commend)
            {
                case "Encodefile": Encodefile(); break;
                case "testjob": Testjob(); break;
                default: Console.WriteLine(@"Not found this ""{0}"" commend", Commend); break;
            }
        }
        public void Encodefile()
        {
            Console.WriteLine("输入需要加密的文件的路径");
            var filepath = Console.ReadLine();
            Console.WriteLine("输入加密后文件的名字");
            var filename = Console.ReadLine();
            Console.WriteLine("输入保存路径");
            var SavePath = Console.ReadLine();
            byte[] fileBytes = F_IO.ReadBytefile(filepath);
            var DeTuple = DataEncryption.RandomAesEncrypt(fileBytes);
            F_IO.CreateAndWrite(string.Format(@"{0}\{1}.txt", SavePath, filename), DeTuple.Item1);
            F_IO.CreateAndWrite(string.Format(@"{0}\{1}.key", SavePath, filename), DeTuple.Item2);
            F_IO.CreateAndWrite(string.Format(@"{0}\{1}.iv", SavePath, filename), DeTuple.Item3);
            Console.WriteLine("加密完成，文件输出在 {0}", SavePath);
        }
        public void Testjob()
        {
            byte[] ts = new byte[] { 12, 23, 45, 67, 89, 80, };
            using DataProcessJod job1 = new DataProcessJod() { Data = ts, State = true };
            DataProcessing.GetInstance().AddData(job1);
        }
    }
}
