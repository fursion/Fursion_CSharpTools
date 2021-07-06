using System;
using System.Linq;
using Fursion_CSharpTools;
using Fursion_CSharpTools.Tools;
using Fursion_CSharpTools.Net.Server;
using Fursion_CSharpTools.Net.Public;
using Fursion_CSharpTools.AsyncJob;
using System.Collections;
using System.Text;
using System.Buffers.Text;

namespace GameServerMain
{
    class Program
    {
        struct Testjob : IJobTask
        {
            public void CallBack(object obj)
            {
                throw new NotImplementedException();
            }

            public void Execute(object obj)
            {
                FDebug.Log("jobTask test");
            }
        }
        static void Main(string[] args)
        {
            var s = Service_IOSystem.CreateMySQLConnectionStatement("TankTest", "cdb-ahtsamo2.cd.tencentcdb.com", "root", "Dj199706194430", 10000);
            Service_IOSystem.GetInstance().ConnectSQL(s);
            Console.Title = "GameService";
            TCPConnectMonitor.GetInstance().StarServer("127.0.0.1", 1024, SocketCall);
            while (true)
            {
                ServiceCommand.GetInstance().CheckCommand();
            }
        }
        public static void SocketCall(byte[] bs)
        {

            FDebug.Log("收到信号");
        }
        public static void test()
        {
            for (int i = 0; i < 10000; i++)
            {
                byte[] ts = new byte[] { (byte)i, (byte)i, (byte)i, (byte)i, (byte)i, (byte)i, (byte)i, (byte)i };
                using DataProcessJod job1 = new DataProcessJod() { Data = ts, State = true };
                DataProcessing.GetInstance().AddData(job1);
            }
        }
    }
}
