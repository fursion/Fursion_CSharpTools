using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.IO;
using Fursion_CSharpTools.Tools;
using Fursion_CSharpTools.AsyncJob;

namespace Fursion_CSharpTools.Core
{
    public class MailPush
    {
        public struct VerifyeMail : IJobTask
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
                FDebug.Log("发送完成");
            }

            public void Execute(object obj)
            {
                try
                {
                    MailMessage mailMsg = new MailMessage();
                    mailMsg.From = new MailAddress("fursion@mailpush.fursion.cn", "fursion");
                    mailMsg.To.Add(new MailAddress(Addressee));
                    //mailMsg.CC.Add("抄送人地址");
                    //mailMsg.Bcc.Add("密送人地址");
                    //可选，设置回信地址 
                    mailMsg.ReplyToList.Add("fursion@fursion.cn");
                    // 邮件主题
                    mailMsg.Subject = Subject;
                    // 邮件正文内容
                    mailMsg.Body = MailText;
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(MailText, null, MediaTypeNames.Text.Plain));
                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(MailHtml, null, MediaTypeNames.Text.Html));
                    // 添加附件
                    if (System.IO.File.Exists(File))
                    {
                        Attachment data = new Attachment(File, MediaTypeNames.Application.Octet);
                        mailMsg.Attachments.Add(data);
                    }
                    //邮件推送的SMTP地址和端口
                    SmtpClient smtpClient = new SmtpClient("smtpdm.aliyun.com", 25);
                    //C#官方文档介绍说明不支持隐式TLS方式，即465端口，需要使用25或者80端口(ECS不支持25端口)，另外需增加一行 smtpClient.EnableSsl = true; 故使用SMTP加密方式需要修改如下：
                    //SmtpClient smtpClient = new SmtpClient("smtpdm.aliyun.com", 80);
                    //smtpClient.EnableSsl = true;
                    // 使用SMTP用户名和密码进行验证
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("fursion@mailpush.fursion.cn", "FUrsion20210710");
                    smtpClient.Credentials = credentials;
                    smtpClient.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    FDebug.Log(ex.ToString());
                }
            }
        }

        private  static void Send()
        {
            try
            {
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress("fursion@mailpush.fursion.cn", "fursion");
                mailMsg.To.Add(new MailAddress("604357968@qq.com"));
                //mailMsg.CC.Add("抄送人地址");
                //mailMsg.Bcc.Add("密送人地址");
                //可选，设置回信地址 
                mailMsg.ReplyToList.Add("fursion@fursion.cn");
                // 邮件主题
                mailMsg.Subject = "邮件主题C#测试";
                // 邮件正文内容
                string text = "欢迎使用阿里云邮件推送";
                string html = @"欢迎使用<a href=""https://dm.console.aliyun.com"">邮件推送</a>";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));
                // 添加附件
                string file = "D:\\1.txt";
                Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                mailMsg.Attachments.Add(data);
                //邮件推送的SMTP地址和端口
                SmtpClient smtpClient = new SmtpClient("smtpdm.aliyun.com", 25);
                //C#官方文档介绍说明不支持隐式TLS方式，即465端口，需要使用25或者80端口(ECS不支持25端口)，另外需增加一行 smtpClient.EnableSsl = true; 故使用SMTP加密方式需要修改如下：
                //SmtpClient smtpClient = new SmtpClient("smtpdm.aliyun.com", 80);
                //smtpClient.EnableSsl = true;
                // 使用SMTP用户名和密码进行验证
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("fursion@mailpush.fursion.cn", "FUrsion20210710");
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
                Console.WriteLine("发送完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static void SendNewVerifyMail()
        {
            VerifyeMail verifyeMail = new VerifyeMail("验证码", "234890", @"欢迎使用<a href=""https://dm.console.aliyun.com"">邮件推送</a>", @"D:\OneDrive\桌面\log\杜洁2021年5月1.5倍加班.docx", "604357968@qq.com");
            TaskCore.Run(verifyeMail);
        }
    }
}
