using DbusSmsForward.Helper;
using DbusSmsForward.ProcessSmsContent;
using DbusSmsForward.SMSModel;
using System.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DbusSmsForward.SendMethod
{
    public static class SendByPushPlus
    {
        public static void SetupPushPlusInfo()
        {
            string pushPlusToken = ConfigurationManager.AppSettings["pushPlusToken"];
            if (string.IsNullOrEmpty(pushPlusToken))
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Console.WriteLine("首次运行请输入PushPlusToken：");
                pushPlusToken = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["pushPlusToken"].Value = pushPlusToken;
                cfa.Save();
            }
        }

        public static void SendSms(SmsContentModel smsmodel, string body)
        {
            ConfigurationManager.RefreshSection("appSettings");
            string pushPlusToken = ConfigurationManager.AppSettings["pushPlusToken"];
            string pushPlusUrl = "http://www.pushplus.plus/send/";
            string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);

            JsonObject obj = new JsonObject();
            obj.Add("token", pushPlusToken);
            obj.Add("title", (string.IsNullOrEmpty(SmsCodeStr) ? "" : SmsCodeStr+" ") + "短信转发" + smsmodel.TelNumber);
            obj.Add("content", body);
            obj.Add("topic","");
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
    }
}
