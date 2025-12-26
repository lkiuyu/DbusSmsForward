using DbusSmsForward.Helper;
using DbusSmsForward.ProcessSmsContent;
using DbusSmsForward.SettingModel;
using DbusSmsForward.SMSModel;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DbusSmsForward.SendMethod
{
    public static class SendByPushPlus
    {
        public static void SetupPushPlusInfo()
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string pushPlusToken = result.appSettings.PushPlusConfig.pushPlusToken;
            if (string.IsNullOrEmpty(pushPlusToken))
            {
                Console.WriteLine("首次运行请输入PushPlusToken：");
                pushPlusToken = Console.ReadLine().Trim();
                result.appSettings.PushPlusConfig.pushPlusToken = pushPlusToken;
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
                string pushPlusToken = result.appSettings.PushPlusConfig.pushPlusToken;
                result = null;
                string pushPlusUrl = "http://www.pushplus.plus/send/";
                //string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);
                SmsCodeModel codeResult = GetSmsContentCode.GetSmsCodeModel(smsmodel.SmsContent);

                JsonObject obj = new JsonObject();
                obj.Add("token", pushPlusToken);
                obj.Add("title", (string.IsNullOrEmpty(codeResult.CodeValue) ? "" : codeResult.CodeValue + " ") + "短信转发" + smsmodel.TelNumber);
                obj.Add("content", body);
                obj.Add("topic", "");
                string msgresult = HttpHelper.Post(pushPlusUrl, obj);
                //JObject jsonObjresult = JObject.Parse(msgresult);
                JsonObject jsonObjresult = JsonSerializer.Deserialize(msgresult, SourceGenerationContext.Default.JsonObject);
                string code = jsonObjresult["code"].ToString();
                string errmsg = jsonObjresult["msg"].ToString();
                if (code == "200")
                {
                    Console.WriteLine("pushplus转发成功");
                }
                else
                {
                    Console.WriteLine(errmsg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetupPushPlusInfoError:\n" + ex);
            }
            
        }
    }
}
