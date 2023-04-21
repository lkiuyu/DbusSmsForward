using DbusSmsForward.Helper;
using Newtonsoft.Json.Linq;
using System.Configuration;

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

        public static void SendSms(string number, string body)
        {
            ConfigurationManager.RefreshSection("appSettings");
            string pushPlusToken = ConfigurationManager.AppSettings["pushPlusToken"];
            string pushPlusUrl = "http://www.pushplus.plus/send/";
            JObject obj = new JObject();
            obj.Add("token", pushPlusToken);
            obj.Add("title", "短信转发"+ number);
            obj.Add("content", body);
            obj.Add("topic","");
            string msgresult = HttpHelper.Post(pushPlusUrl, obj);
            JObject jsonObjresult = JObject.Parse(msgresult);
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
