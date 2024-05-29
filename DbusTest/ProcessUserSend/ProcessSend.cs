using DbusSmsForward.SendMethod;
using DbusSmsForward.SMSModel;

namespace DbusSmsForward.ProcessUserSend
{
    public class ProcessSend
    {
        public static void sendSms(string sendMethodGuideResult,SmsContentModel smsmodel)
        {
            string body = "发信电话:" + smsmodel.TelNumber + "\n" + "时间:" + smsmodel.SmsDate + "\n" + "短信内容:" + smsmodel.SmsContent;
            Console.WriteLine(body);
            try
            {
                if (sendMethodGuideResult == "1")
                {
                    SendByEmail.SendSms(smsmodel, body);
                }
                if (sendMethodGuideResult == "2")
                {
                    SendByPushPlus.SendSms(smsmodel, body);
                }
                if (sendMethodGuideResult == "3")
                {
                    SendByWeComApplication.SendSms(smsmodel, body);
                }
                if (sendMethodGuideResult == "4")
                {
                    SendByTelegramBot.SendSms(smsmodel, body);
                }
                if(sendMethodGuideResult == "5")
                {
                    SendByDingTalkBot.SendSms(smsmodel, body);
                }
                if (sendMethodGuideResult == "6")
                {
                    SendByBark.SendSms(smsmodel, body);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
