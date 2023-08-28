using DbusSmsForward.SendMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbusSmsForward.ProcessUserSend
{
    public class ProcessSend
    {
        public static void sendSms(string sendMethodGuideResult,string tel,string body)
        {
            try
            {
                if (sendMethodGuideResult == "1")
                {
                    SendByEmail.SendSms(tel, body);
                }
                if (sendMethodGuideResult == "2")
                {
                    SendByPushPlus.SendSms(tel, body);
                }
                if (sendMethodGuideResult == "3")
                {
                    SendByWeComApplication.SendSms(tel, body);
                }
                if (sendMethodGuideResult == "4")
                {
                    SendByTelegramBot.SendSms(tel, body);
                }
                if(sendMethodGuideResult == "5")
                {
                    SendByDingTalkBot.SendSms(tel, body);
                }
                if (sendMethodGuideResult == "6")
                {
                    SendByBark.SendSms(tel, body);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
