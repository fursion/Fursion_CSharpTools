using System;
using System.Linq;
using Fursion_CSharpTools.Core;
using Fursion_CSharpTools.Core.FileTransfer;
using Fursion_CSharpTools;
using Fursion_CSharpTools.Tools;
using Fursion_CSharpTools.Net.Server;
using Fursion_CSharpTools.Net.Public;
using Fursion_CSharpTools.Net.UDP;
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
            SecurityManagement.NowUserRights = SystemRights.root;
            var s = Service_IOSystem.CreateMySQLConnectionStatement("TankTest", "cdb-ahtsamo2.cd.tencentcdb.com", "root", "Dj199706194430", 10000);
            Service_IOSystem.GetInstance().ConnectSQL(s);
            Console.Title = "GameService";
            TCPConnectMonitor.GetInstance().StarServer("127.0.0.1", 1024, SocketCall);
            SecurityManagement.Rights();
            FileTransfer.FileSlicing();
            UDPMonitor uDPMonitor = new UDPMonitor();
            uDPMonitor.SocketInit("127.0.0.1",3500);
            while (true)
            {
                ServiceCommand.GetInstance().CheckCommand();
            }
        }
        public static void SocketCall(byte[] bs)
        {

            FDebug.Log("收到信号");
        }
     
    }
}
