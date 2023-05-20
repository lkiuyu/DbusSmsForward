using DbusSmsForward.Helper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbusSmsForward.SendMethod
{
    public static class SendByTelegramBot
    {
        public static void SetupTGBotInfo()
        {
            string TGBotToken = ConfigurationManager.AppSettings["TGBotToken"];
            string TGBotChatID = ConfigurationManager.AppSettings["TGBotChatID"];

            if (string.IsNullOrEmpty(TGBotToken) && string.IsNullOrEmpty(TGBotChatID))
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Console.WriteLine("首次运行请输入TG机器人Token：");
                TGBotToken = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["TGBotToken"].Value = TGBotToken;
                Console.WriteLine("首次运行请输入机器人要转发到的ChatId");
                TGBotChatID = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["TGBotChatID"].Value = TGBotChatID;
                cfa.Save();
            }
        }

        public static void SendSms(string number, string body)
        {
            ConfigurationManager.RefreshSection("appSettings");
            string TGBotToken = ConfigurationManager.AppSettings["TGBotToken"];
            string TGBotChatID = ConfigurationManager.AppSettings["TGBotChatID"];
            string url = "https://api.telegram.org/bot" + TGBotToken + "/sendMessage?chat_id=" + TGBotChatID + "&text=";
            url += System.Web.HttpUtility.UrlEncode(body);
            string msgresult = HttpHelper.HttpGet(url);
            JObject jsonObjresult = JObject.Parse(msgresult);
            string status = jsonObjresult["ok"].ToString();
            if (status == "True")
            {
                Console.WriteLine("TGBot转发成功");
            }
            else
            {
                Console.WriteLine(jsonObjresult["error_code"].ToString());
                Console.WriteLine(jsonObjresult["description"].ToString());
            }

        }

        
    }
}
