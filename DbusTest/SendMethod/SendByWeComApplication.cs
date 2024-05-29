using DbusSmsForward.Helper;
using DbusSmsForward.ProcessSmsContent;
using DbusSmsForward.SMSModel;
using System.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace DbusSmsForward.SendMethod
{
    public class SendByWeComApplication
    {
        public static void SetupWeComInfo()
        {
            string corpid = ConfigurationManager.AppSettings["WeChatQYID"];
            string appsecret = ConfigurationManager.AppSettings["WeChatQYApplicationSecret"];
            string appid = ConfigurationManager.AppSettings["WeChatQYApplicationID"];
            if (string.IsNullOrEmpty(corpid) && string.IsNullOrEmpty(appsecret) && string.IsNullOrEmpty(appid))
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Console.WriteLine("首次运行请输入企业ID：");
                corpid = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["WeChatQYID"].Value = corpid;

                Console.WriteLine("请输入自建应用ID：");
                appid = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["WeChatQYApplicationID"].Value = appid;

                Console.WriteLine("请输入自建应用密钥：");
                appsecret = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["WeChatQYApplicationSecret"].Value = appsecret;

                cfa.Save();
            }
        }

        public static void SendSms(SmsContentModel smsmodel, string body)
        {
            ConfigurationManager.RefreshSection("appSettings");
            string corpid = ConfigurationManager.AppSettings["WeChatQYID"];
            string corpsecret = ConfigurationManager.AppSettings["WeChatQYApplicationSecret"];
            int agentid =Convert.ToInt32(ConfigurationManager.AppSettings["WeChatQYApplicationID"]);
            string url = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" + corpid + "&corpsecret=" + corpsecret;
            string result = HttpHelper.HttpGet(url);
            //JsonObject jsonObj = JsonObject.Parse(result);
            JsonObject jsonObj = JsonSerializer.Deserialize(result, SourceGenerationContext.Default.JsonObject);
            string errcode = jsonObj["errcode"].ToString();
            string errmsg = jsonObj["errmsg"].ToString();
            string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);
            if (errcode == "0" && errmsg == "ok")
            {
                string access_token = jsonObj["access_token"].ToString();
                JsonObject obj = new JsonObject();
                JsonObject obj1 = new JsonObject();
                obj.Add("touser", "@all");
                obj.Add("toparty", "");
                obj.Add("totag", "");
                obj.Add("msgtype", "text");
                obj.Add("agentid", agentid);
                obj1.Add("content", (string.IsNullOrEmpty(SmsCodeStr) ? "" : SmsCodeStr+"\n")+"短信转发\n" + body);
                obj.Add("text", obj1);
                obj.Add("safe", 0);
                obj.Add("enable_id_trans", 0);
                obj.Add("enable_duplicate_check", 0);
                obj.Add("duplicate_check_interval", 1800);
                string msgurl = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token=" + access_token;
                string msgresult = HttpHelper.Post(msgurl, obj);
                //JsonObject jsonObjresult = JsonObject.Parse(msgresult);
                //JsonObject jsonObjresult = JsonSerializer.Deserialize<JsonObject>(msgresult);
                JsonObject jsonObjresult = JsonSerializer.Deserialize(msgresult, SourceGenerationContext.Default.JsonObject);
                string errcode1 = jsonObjresult["errcode"].ToString();
                string errmsg1 = jsonObjresult["errmsg"].ToString();
                if (errcode1 == "0" && errmsg1 == "ok")
                {
                    Console.WriteLine("企业微信转发成功");
                }
                else
                {
                    Console.WriteLine(errmsg1);
                }
            }
            else
            {
                Console.WriteLine(errmsg);
            }

        }
    }
}
