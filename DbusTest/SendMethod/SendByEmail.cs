using System.Net.Mail;
using System.Net;
using DbusSmsForward.SMSModel;
using DbusSmsForward.ProcessSmsContent;
using DbusSmsForward.Helper;
using DbusSmsForward.SettingModel;

namespace DbusSmsForward.SendMethod
{
    public static class SendByEmail
    {
        public static void SetupEmailInfo()
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string smtpHost = result.appSettings.EmailConfig.smtpHost;
            string smtpPort = result.appSettings.EmailConfig.smtpPort;
            string emailKey = result.appSettings.EmailConfig.emailKey;
            string sendEmial = result.appSettings.EmailConfig.sendEmial;
            string reciveEmial = result.appSettings.EmailConfig.reciveEmial;
            if (string.IsNullOrEmpty(smtpHost) && string.IsNullOrEmpty(smtpPort) && string.IsNullOrEmpty(emailKey) && string.IsNullOrEmpty(sendEmial) && string.IsNullOrEmpty(reciveEmial))
            {
                Console.WriteLine("首次运行请输入邮箱转发相关配置信息\n请输入smtp地址：");
                smtpHost = Console.ReadLine().Trim();
                result.appSettings.EmailConfig.smtpHost = smtpHost;

                Console.WriteLine("请输入smtp端口：");
                smtpPort = Console.ReadLine().Trim();
                result.appSettings.EmailConfig.smtpPort = smtpPort;

                Console.WriteLine("请输入邮箱密钥：");
                emailKey = Console.ReadLine().Trim();
                result.appSettings.EmailConfig.emailKey = emailKey;

                Console.WriteLine("请输入发件邮箱：");
                sendEmial = Console.ReadLine().Trim();
                result.appSettings.EmailConfig.sendEmial = sendEmial;

                Console.WriteLine("请输入收件邮箱：");
                reciveEmial = Console.ReadLine().Trim();
                result.appSettings.EmailConfig.reciveEmial = reciveEmial;

                ConfigHelper.UpdateSettings(ref result);
            }
            result = null;

        }


        public static void SendSms(SmsContentModel smsmodel, string body)
        {

            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string smtpHost = result.appSettings.EmailConfig.smtpHost;
            string smtpPort = result.appSettings.EmailConfig.smtpPort;
            string emailKey = result.appSettings.EmailConfig.emailKey;
            string sendEmial = result.appSettings.EmailConfig.sendEmial;
            string reciveEmial = result.appSettings.EmailConfig.reciveEmial;
            result = null;
            MailAddress to = new MailAddress(reciveEmial);
            MailAddress from = new MailAddress(sendEmial, "SMSForwad");
            MailMessage mm = new MailMessage(from, to);
            SmtpClient sc = new SmtpClient(smtpHost);
            try
            {
                string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);
                mm.Subject = (string.IsNullOrEmpty(SmsCodeStr)?"": SmsCodeStr + " ") + "短信转发" + smsmodel.TelNumber;
                mm.Body = body;
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                sc.Credentials = new NetworkCredential(sendEmial, emailKey);
                sc.Send(mm);
                Console.WriteLine("邮箱转发成功");
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
