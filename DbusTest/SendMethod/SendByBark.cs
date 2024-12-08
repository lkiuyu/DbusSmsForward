using DbusSmsForward.Helper;
using DbusSmsForward.ProcessSmsContent;
using DbusSmsForward.SettingModel;
using DbusSmsForward.SMSModel;
using System.Text.Json;

namespace DbusSmsForward.SendMethod
{
    public static class SendByBark
    {
        public static void SetupBarkInfo()
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string BarkUrl = result.appSettings.BarkConfig.BarkUrl;
            string BrakKey = result.appSettings.BarkConfig.BrakKey;

            if (string.IsNullOrEmpty(BarkUrl) && string.IsNullOrEmpty(BrakKey))
            {
                Console.WriteLine("首次运行请输入Bark服务器地址：");
                BarkUrl = Console.ReadLine().Trim();
                result.appSettings.BarkConfig.BarkUrl = BarkUrl;
                Console.WriteLine("首次运行请输入Bark推送key");
                BrakKey = Console.ReadLine().Trim();
                result.appSettings.BarkConfig.BrakKey = BrakKey;
                ConfigHelper.UpdateSettings(ref result);
            }
            result = null;
        }

        public static void SendSms(SmsContentModel smsmodel, string body)
        {
            try
            {
                appsettingsModel result = new appsettingsModel();
                ConfigHelper.GetSettings(ref result);
                string BarkUrl = result.appSettings.BarkConfig.BarkUrl;
                string BrakKey = result.appSettings.BarkConfig.BrakKey;
                result = null;
                string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);
                string url = BarkUrl + "/" + BrakKey + "/";
                url += System.Web.HttpUtility.UrlEncode(body);
                url += "?group=" + smsmodel.TelNumber + "&title="+ (string.IsNullOrEmpty(SmsCodeStr) ? "" : SmsCodeStr + " ") + "短信转发" + smsmodel.TelNumber+(string.IsNullOrEmpty(SmsCodeStr)?"":"&autoCopy=1&copy="+ GetSmsContentCode.GetSmsCodeModel(smsmodel.SmsContent).CodeValue);
                string msgresult = HttpHelper.HttpGet(url);
                //JsonObject jsonObjresult = JObject.Parse(msgresult);
                var jsonObjresult = JsonSerializer.Deserialize(msgresult, SourceGenerationContext.Default.JsonObject);
                string status = jsonObjresult["code"].ToString();
                if (status == "200")
                {
                    Console.WriteLine("Bark转发成功");
                }
                else
                {
                    Console.WriteLine(jsonObjresult["code"].ToString());
                    Console.WriteLine(jsonObjresult["message"].ToString());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }
}
