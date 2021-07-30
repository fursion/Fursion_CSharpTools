using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.IO;
using Fursion_CSharpTools.Tools;
using Fursion_CSharpTools.AsyncJob;
using System.Threading.Tasks;

namespace Fursion_CSharpTools.Core
{
    public class MailPush
    {
        public struct VerifyeMail : IJobTaskGet<string>
        {
            private string _subject;
            public string Subject { get { return _subject; } set { _subject = value; } }
            public string _mailtext;
            public string MailText { get { return _mailtext; } set { _mailtext = value; } }
            private string _mailhtml;
            public string MailHtml { get { return _mailhtml; } set { _mailhtml = value; } }
            private string _file;
            public string File { get { return _file; } set { _file = value; } }
            private string _addressee;
            public string Addressee { get { return _addressee; } set { _addressee = value; } }
            public VerifyeMail(string subject, string mailtext, string mailhtml, string file, string addressee)
            {
                this._subject = subject; this._mailtext = mailtext; this._mailhtml = mailhtml; this._file = file; this._addressee = addressee;
            }
            public void CallBack(object obj)
            {
                FDebug.Log(((Task)obj).Status.ToString());
            }

            string IJobTaskGet<string>.Execute(object obj)
            {
                try
                {
                    MailMessage mailMsg = new MailMessage();
                    mailMsg.From = new MailAddress("mailpush@mail.fursion.cn", "fursion");
                    mailMsg.To.Add(new MailAddress(Addressee));
                    //mailMsg.CC.Add("抄送人地址");
                    //mailMsg.Bcc.Add("密送人地址");
                    //可选，设置回信地址 
                    mailMsg.ReplyToList.Add("fursion@fursion.cn");
                    // 邮件主题
                    mailMsg.Subject = Subject;
                    // 邮件正文内容
                    //mailMsg.Body = MailText;
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(MailText, null, MediaTypeNames.Text.Html));
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(MailHtml, null, MediaTypeNames.Text.Html));
                    // 添加附件
                    if (System.IO.File.Exists(File))
                    {
                        Attachment data = new Attachment(File, MediaTypeNames.Application.Octet);
                        mailMsg.Attachments.Add(data);
                    }
                    //邮件推送的SMTP地址和端口
                    SmtpClient smtpClient = new SmtpClient("mail.fursion.cn", 25);
                    //smtpClient.EnableSsl = true;
                    // 使用SMTP用户名和密码进行验证
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("mailpush@mail.fursion.cn", "dj970619");
                    smtpClient.Credentials = credentials;
                    smtpClient.Send(mailMsg);
                    return "send successfuly";
                }
                catch (Exception ex)
                {
                    FDebug.Log(ex.ToString());
                    return ex.Message;
                }
            }
        }
        public static void sendmail()
        {
 
        }
        public static async Task SendNewVerifyMailAsync()
        {
            VerifyeMail verifyeMail = new VerifyeMail("验证码", CSharpTools.CreateUUID().ToString(), "", null, "604357968@qq.com");
            await TaskCore.Run(verifyeMail);
        }
    }
}
