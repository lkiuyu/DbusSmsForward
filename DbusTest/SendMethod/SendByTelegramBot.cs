using DbusSmsForward.Helper;
using DbusSmsForward.ProcessSmsContent;
using DbusSmsForward.SettingModel;
using DbusSmsForward.SMSModel;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DbusSmsForward.SendMethod
{
    public static class SendByTelegramBot
    {
        public static void SetupTGBotInfo()
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string TGBotToken = result.appSettings.TGBotConfig.TGBotToken;
            string TGBotChatID = result.appSettings.TGBotConfig.TGBotChatID;
            string IsEnableCustomTGBotApi = result.appSettings.TGBotConfig.IsEnableCustomTGBotApi;
            string CustomTGBotApi = result.appSettings.TGBotConfig.CustomTGBotApi;

            if (string.IsNullOrEmpty(TGBotToken) || string.IsNullOrEmpty(TGBotChatID) || string.IsNullOrEmpty(IsEnableCustomTGBotApi))
            {
                Console.WriteLine("首次运行请输入TG机器人Token：");
                TGBotToken = Console.ReadLine().Trim();
                result.appSettings.TGBotConfig.TGBotToken = TGBotToken;
                Console.WriteLine("请输入机器人要转发到的ChatId");
                TGBotChatID = Console.ReadLine().Trim();
                result.appSettings.TGBotConfig.TGBotChatID = TGBotChatID;

                string customApiEnableInput=string.Empty;
                do
                {
                    Console.WriteLine("是否需要使用自定义api(1.使用 2.不使用)");
                    customApiEnableInput= Console.ReadLine().Trim();
                } while (!(customApiEnableInput=="1"|| customApiEnableInput == "2"));
                if(customApiEnableInput=="1")
                {
                    IsEnableCustomTGBotApi = "true";
                    result.appSettings.TGBotConfig.IsEnableCustomTGBotApi = IsEnableCustomTGBotApi;
                    Console.WriteLine("请输入机器人自定义api(格式https://xxx.abc.com)");
                    CustomTGBotApi= Console.ReadLine().Trim();
                    result.appSettings.TGBotConfig.CustomTGBotApi = CustomTGBotApi;
                }
                else
                {
                    IsEnableCustomTGBotApi = "false";
                    result.appSettings.TGBotConfig.IsEnableCustomTGBotApi = IsEnableCustomTGBotApi;
                }
                ConfigHelper.UpdateSettings(ref result);
            }
            result = null;

        }

        public static void SendSms(SmsContentModel smsmodel, string body)
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string TGBotToken = result.appSettings.TGBotConfig.TGBotToken;
            string TGBotChatID = result.appSettings.TGBotConfig.TGBotChatID;
            string IsEnableCustomTGBotApi = result.appSettings.TGBotConfig.IsEnableCustomTGBotApi;
            string CustomTGBotApi = result.appSettings.TGBotConfig.CustomTGBotApi;
            result = null;
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
            JsonObject jsonObjresult = JsonSerializer.Deserialize(msgresult, SourceGenerationContext.Default.JsonObject);
            string status = jsonObjresult["ok"].ToString();
            if (status.ToLower() == "true")
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
