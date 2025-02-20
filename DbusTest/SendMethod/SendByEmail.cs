using DbusSmsForward.SMSModel;
using DbusSmsForward.ProcessSmsContent;
using DbusSmsForward.Helper;
using DbusSmsForward.SettingModel;
using MimeKit;
using MailKit.Net.Smtp;

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
                string sslEnableInput = string.Empty;
                do
                {
                    Console.WriteLine("是否需要启用ssl(1.启用 2.不启用)");
                    sslEnableInput = Console.ReadLine().Trim();
                } while (!(sslEnableInput == "1" || sslEnableInput == "2"));
                if (sslEnableInput == "1")
                {
                    result.appSettings.EmailConfig.enableSSL = "true";
                }
                else
                {
                    result.appSettings.EmailConfig.enableSSL = "false";
                }
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
            bool enableSSL = Convert.ToBoolean(result.appSettings.EmailConfig.enableSSL);
            string emailKey = result.appSettings.EmailConfig.emailKey;
            string sendEmial = result.appSettings.EmailConfig.sendEmial;
            string reciveEmial = result.appSettings.EmailConfig.reciveEmial;
            result = null;
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SMSForwad", sendEmial));
            message.To.Add(new MailboxAddress("", reciveEmial));
            string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);
            message.Subject = (string.IsNullOrEmpty(SmsCodeStr) ? "" : SmsCodeStr + " ") + "短信转发" + smsmodel.TelNumber;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {
                client.Connect(smtpHost, Convert.ToInt32(smtpPort), enableSSL);
                client.Authenticate(sendEmial, emailKey);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
