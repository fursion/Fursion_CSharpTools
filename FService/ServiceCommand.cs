using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Fursion_CSharpTools;
using Fursion_CSharpTools.Net.Public;
using Fursion_CSharpTools.Tools;
using Fursion_CSharpTools.AsyncJob;

namespace GameServerMain
{
    class ServiceCommand : Singleton<ServiceCommand>
    {
        struct Testjob1 : IJobTask
        {
            public int i;
            public void CallBack(object obj)
            {
                FDebug.Log("回调 {0}  线程ID：{1}",i*i,Thread.CurrentThread.ManagedThreadId);
            }

            public void Execute(object obj)
            {
                FDebug.Log("{0}   线程ID：{1}", i,Thread.CurrentThread.ManagedThreadId);
            }
        }
        struct TestjobGet1 : IJobTaskGet<int>
        {
            public int i;
            public void CallBack(object obj)
            {
                FDebug.Log("回调 {0}  线程ID：{1}", i * i, Thread.CurrentThread.ManagedThreadId);
            }
            public int Execute(object obj)
            {
                FDebug.Log("执行任务");
                return Thread.CurrentThread.ManagedThreadId*i;
            }
        }

        public void CheckCommand()
        {
            string Commend = Console.ReadLine();
            switch (Commend)
            {
                case "Encodefile": Encodefile(); break;
                case "testjob": Testjob(); break;
                case "jobtask": JobTest(); break;
                default: Console.WriteLine(@"Not found this ""{0}"" command", Commend); break;
            }
        }
        public async void JobTest()
        {
            //for (int i = 0; i < 3000; i++)
            //{
            //    Testjob1 testjob = new Testjob1();
            //    testjob.i = i;
            //    TaskCore.Run(testjob);
            //}
            TestjobGet1 testjobGet1 = new TestjobGet1();
            testjobGet1.i = 2;
            var tr= await TaskCore.Run(testjobGet1);
            Console.WriteLine(tr);
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

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
