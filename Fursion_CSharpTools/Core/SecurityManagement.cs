using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Fursion_CSharpTools.Core
{
    public enum SystemRights
    {
        root,
        Admin,
        User
    }
    public class SecurityManagement
    {
        private static SystemRights _NowRights;
        public static SystemRights NowUserRights { get { return _NowRights; } set { _NowRights = value; } }
        [Rightslimt(SystemRights.Admin)]
        public static bool Rights()
        {
            var right = MethodBase.GetCurrentMethod().GetCustomAttribute<Rightslimt>()?.CheckRights();
            Console.WriteLine(right);
            return false;
        }
        public static void CheckRights()
        {

        }
        public SystemRights GetNetUserRights(string userid)
        {
            return SystemRights.User;
        }
    }
}
