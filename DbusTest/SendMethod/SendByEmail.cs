using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DbusSmsForward.SendMethod
{
    public static class SendByEmail
    {
        public static void SendSms(string number,string body)
        {
            string smtpHost = ConfigurationManager.AppSettings["smtpHost"];
            string smtpPort = ConfigurationManager.AppSettings["smtpPort"];
            string emailKey = ConfigurationManager.AppSettings["emailKey"];
            string sendEmial = ConfigurationManager.AppSettings["sendEmial"];
            string reciveEmial = ConfigurationManager.AppSettings["reciveEmial"];

            MailAddress to = new MailAddress(reciveEmial);
            MailAddress from = new MailAddress(sendEmial, "SMSForwad");
            MailMessage mm = new MailMessage(from, to);
            SmtpClient sc = new SmtpClient(smtpHost);
            try
            {
                mm.Subject = "短信转发" + number;
                mm.Body = body;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.Credentials = new NetworkCredential(sendEmial, emailKey);
                sc.Send(mm);
                Console.WriteLine("转发成功");
                mm.Dispose();
                sc.Dispose();
            }
            catch (SmtpException ex)
            {
                mm.Dispose();
                sc.Dispose();
                Console.WriteLine(ex);
                Console.WriteLine("出错了，尝试确认下配置文件中的邮箱信息是否正确，配置文件为DbusSmsForward.dll.config");
            }
        }
    }
}
