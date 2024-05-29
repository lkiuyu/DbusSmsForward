using DbusSmsForward.Helper;
using DbusSmsForward.ProcessSmsContent;
using DbusSmsForward.SMSModel;
using System.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DbusSmsForward.SendMethod
{
    public static class SendByTelegramBot
    {
        public static void SetupTGBotInfo()
        {
            string TGBotToken = ConfigurationManager.AppSettings["TGBotToken"];
            string TGBotChatID = ConfigurationManager.AppSettings["TGBotChatID"];
            string IsEnableCustomTGBotApi = ConfigurationManager.AppSettings["IsEnableCustomTGBotApi"];
            string CustomTGBotApi = ConfigurationManager.AppSettings["CustomTGBotApi"];


            if (string.IsNullOrEmpty(TGBotToken) && string.IsNullOrEmpty(TGBotChatID)&& string.IsNullOrEmpty(IsEnableCustomTGBotApi))
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Console.WriteLine("首次运行请输入TG机器人Token：");
                TGBotToken = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["TGBotToken"].Value = TGBotToken;
                Console.WriteLine("请输入机器人要转发到的ChatId");
                TGBotChatID = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["TGBotChatID"].Value = TGBotChatID;

                string customApiEnableInput=string.Empty;
                do
                {
                    Console.WriteLine("是否需要使用自定义api(1.使用 2.不使用)");
                    customApiEnableInput= Console.ReadLine().Trim();
                } while (!(customApiEnableInput=="1"|| customApiEnableInput == "2"));
                if(customApiEnableInput=="1")
                {
                    IsEnableCustomTGBotApi = "true";
                    cfa.AppSettings.Settings["IsEnableCustomTGBotApi"].Value = IsEnableCustomTGBotApi;
                    Console.WriteLine("请输入机器人自定义api(格式https://xxx.abc.com)");
                    CustomTGBotApi= Console.ReadLine().Trim();
                    cfa.AppSettings.Settings["CustomTGBotApi"].Value = CustomTGBotApi;
                }
                else
                {
                    IsEnableCustomTGBotApi = "false";
                    cfa.AppSettings.Settings["IsEnableCustomTGBotApi"].Value = IsEnableCustomTGBotApi;
                }
                cfa.Save();
            }
        }

        public static void SendSms(SmsContentModel smsmodel, string body)
        {
            ConfigurationManager.RefreshSection("appSettings");
            string TGBotToken = ConfigurationManager.AppSettings["TGBotToken"];
            string TGBotChatID = ConfigurationManager.AppSettings["TGBotChatID"];
            string IsEnableCustomTGBotApi = ConfigurationManager.AppSettings["IsEnableCustomTGBotApi"];
            string CustomTGBotApi = ConfigurationManager.AppSettings["CustomTGBotApi"];
            string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);

            string url = "";
            if (IsEnableCustomTGBotApi=="true")
            {
                url = CustomTGBotApi;
            }
            else
            {
                url = "https://api.telegram.org";
            }
            url+= "/bot" + TGBotToken + "/sendMessage?chat_id=" + TGBotChatID + "&text=";
            url += System.Web.HttpUtility.UrlEncode((string.IsNullOrEmpty(SmsCodeStr) ? "" : SmsCodeStr + "\n") + "短信转发\n" + body);
            string msgresult = HttpHelper.HttpGet(url);
            //JsonObject jsonObjresult = JObject.Parse(msgresult);
            JsonObject jsonObjresult = JsonSerializer.Deserialize(msgresult, SourceGenerationContext.Default.JsonObject);
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
