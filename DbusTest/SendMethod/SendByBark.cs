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
            string BrakKey = result.appSettings.BarkConfig.BarkKey;

            if (string.IsNullOrEmpty(BarkUrl) && string.IsNullOrEmpty(BrakKey))
            {
                Console.WriteLine("首次运行请输入Bark服务器地址：");
                BarkUrl = Console.ReadLine().Trim();
                result.appSettings.BarkConfig.BarkUrl = BarkUrl;
                Console.WriteLine("首次运行请输入Bark推送key");
                BrakKey = Console.ReadLine().Trim();
                result.appSettings.BarkConfig.BarkKey = BrakKey;
                ConfigHelper.UpdateSettings(ref result);
            }
            result = null;
        }

        public static void SendSms(SmsContentModel smsmodel, string body, string devicename)
        {
            try
            {
                appsettingsModel result = new appsettingsModel();
                ConfigHelper.GetSettings(ref result);
                string BarkUrl = result.appSettings.BarkConfig.BarkUrl;
                string BrakKey = result.appSettings.BarkConfig.BarkKey;
                result = null;
                //string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);

                SmsCodeModel codeResult = GetSmsContentCode.GetSmsCodeModel(smsmodel.SmsContent);

                string url = BarkUrl + "/" + BrakKey + "/";
                url += System.Web.HttpUtility.UrlEncode(body);
                url += "?group=" + smsmodel.TelNumber + "&title="+ (string.IsNullOrEmpty(codeResult.CodeValue) ? "" : codeResult.CodeValue + " ") + "短信转发" + smsmodel.TelNumber+(string.IsNullOrEmpty(codeResult.CodeValue) ?"":"&autoCopy=1&copy="+ codeResult.CodeValue);
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
