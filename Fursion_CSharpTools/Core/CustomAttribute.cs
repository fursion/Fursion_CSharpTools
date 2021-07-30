using System;
using System.Collections.Generic;
using System.Text;
using Fursion_CSharpTools.Tools;

namespace Fursion_CSharpTools.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Rightslimt : Attribute
    {
        public Rightslimt(SystemRights rights)
        {
            _rights = rights;
        }
        private SystemRights _rights;
        public SystemRights Rights { get { return _rights; } }
        public bool CheckRights()
        {
            if (SecurityManagement.NowUserRights == SystemRights.root || SecurityManagement.NowUserRights == SystemRights.Admin || SecurityManagement.NowUserRights == Rights)
                return true;
            return false;
        }
        public bool CheckRights(SystemRights netRights)
        {
            if (netRights == SystemRights.root || netRights == SystemRights.Admin || netRights == Rights)
                return true;
            FDebug.Log("没有访问权限");
            return false;
        }
        public string ErrorMessage;
    }

}
