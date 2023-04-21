using DbusSmsForward.Helper;
using Newtonsoft.Json.Linq;
using System.Configuration;

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

        public static void SendSms(string number, string body)
        {
            ConfigurationManager.RefreshSection("appSettings");
            string corpid = ConfigurationManager.AppSettings["WeChatQYID"];
            string corpsecret = ConfigurationManager.AppSettings["WeChatQYApplicationSecret"];
            int agentid =Convert.ToInt32(ConfigurationManager.AppSettings["WeChatQYApplicationID"]);
            string url = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" + corpid + "&corpsecret=" + corpsecret;
            string result = HttpHelper.HttpGet(url);
            JObject jsonObj = JObject.Parse(result);
            string errcode = jsonObj["errcode"].ToString();
            string errmsg = jsonObj["errmsg"].ToString();
            if (errcode == "0" && errmsg == "ok")
            {
                string access_token = jsonObj["access_token"].ToString();
                JObject obj = new JObject();
                JObject obj1 = new JObject();
                obj.Add("touser", "@all");
                obj.Add("toparty", "");
                obj.Add("totag", "");
                obj.Add("msgtype", "text");
                obj.Add("agentid", agentid);
                obj1.Add("content", "短信转发\n" + body);
                obj.Add("text", obj1);
                obj.Add("safe", 0);
                obj.Add("enable_id_trans", 0);
                obj.Add("enable_duplicate_check", 0);
                obj.Add("duplicate_check_interval", 1800);
                string msgurl = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token=" + access_token;
                string msgresult = HttpHelper.Post(msgurl, obj);
                JObject jsonObjresult = JObject.Parse(msgresult);
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
