using DbusSmsForward.Helper;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace DbusSmsForward.SendMethod
{
    public class SendByDingTalkBot
    {
        private const string DING_TALK_BOT_URL = "https://oapi.dingtalk.com/robot/send?access_token=";

        private const string KEY_WORD = "===>";
        public static void SetupDingtalkBotMsg() 
        {
            string DingTalkAccessToken = ConfigurationManager.AppSettings["DingTalkAccessToken"];
            if (string.IsNullOrEmpty(DingTalkAccessToken) )
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Console.WriteLine("首次运行请输入钉钉机器人AccessToken：");
                DingTalkAccessToken = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["DingTalkAccessToken"].Value = DingTalkAccessToken;
                cfa.Save();
            }
        }

        public static void SendSms(string number, string body) {
            ConfigurationManager.RefreshSection("appSettings");
            string dingTalkAccessToken = ConfigurationManager.AppSettings["DingTalkAccessToken"];
            string url = DING_TALK_BOT_URL + dingTalkAccessToken;

            JObject msgContent = new()
            {
                { "content", KEY_WORD + body }
            };

            JObject msgObj = new()
            {
                { "msgtype", "text" },
                { "text", msgContent }
            };

            string resultResp =  HttpHelper.Post(url,msgObj);
            JObject jsonObjresult = JObject.Parse(resultResp);
            string errcode1 = jsonObjresult["errcode"].ToString();
            string errmsg1 = jsonObjresult["errmsg"].ToString();
            if (errcode1 == "0" && errmsg1 == "ok")
            {
                Console.WriteLine("钉钉转发成功");
            }
            else
            {
                Console.WriteLine(errmsg1);
            }
        }
    }
}
