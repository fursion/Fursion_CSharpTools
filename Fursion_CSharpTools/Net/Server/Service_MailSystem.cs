using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Fursion_CSharpTools.Net.Server
{
    class Service_MailSystem : Singleton<Service_MailSystem>
    {
        public void Broadcast()
        {

        }
        public void PushMail()
        {

        }
        public void Pull_IO_MailBody()
        {

        }
        public void Pull_IO_Headerlist()
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class Service_MailSystemTool
    {
        private static long LastUUID = 0;
        /// <summary>
        /// 创建邮件的唯一编号
        /// </summary>
        /// <returns></returns>
        public static string CreateMailUUID()
        {
            long NewUUID = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            if (NewUUID <= LastUUID)
                NewUUID = LastUUID + 1;
            LastUUID = NewUUID;
            return NewUUID.ToString();
        }

    }
}
