using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Fursion_CSharpTools;
using Fursion_CSharpTools.Net.Public;
using Fursion_CSharpTools.Core;
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
                FDebug.Log("回调 {0}  线程ID：{1}", i * i, Thread.CurrentThread.ManagedThreadId);
            }

            public void Execute(object obj)
            {
                FDebug.Log("{0}   线程ID：{1}", i, Thread.CurrentThread.ManagedThreadId);
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
                return Thread.CurrentThread.ManagedThreadId * i;
            }
        }

        public void CheckCommand()
        {
            string Commend = Console.ReadLine();           
            var function = Fursion_CSharpTools.CSharpTools.GetMethodDo<ServiceCommand>(Commend);
            if (function != null)
                function.Invoke(this, null);
            else
                FDebug.Log(@"Not found this ""{0}"" command", Commend);
        }
        [TestAttribute("fursion","2021.7.8")]
        public async void JobTest()
        {
            TestjobGet1 testjobGet1 = new TestjobGet1();
            testjobGet1.i = 2;
            var tr = await TaskCore.Run(testjobGet1);
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
        public static void Mailtest()
        {
            MailPush.SendNewVerifyMail();
        }
        public unsafe static void Test()
        {
            DataPacket dataPacket = new DataPacket();  
            Console.WriteLine(CSharpTools.GetObjectSize(dataPacket)); 
        }
        public static void Quit()
        {        
            Environment.Exit(0);
        }
    }
}
