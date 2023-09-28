using DbusSmsForward.Helper;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text;
using System.Web;
using DbusSmsForward.SMSModel;
using DbusSmsForward.ProcessSmsContent;

namespace DbusSmsForward.SendMethod
{
    public class SendByDingTalkBot
    {
        private const string DING_TALK_BOT_URL = "https://oapi.dingtalk.com/robot/send?access_token=";

        public static void SetupDingtalkBotMsg()
        {
            string DingTalkAccessToken = ConfigurationManager.AppSettings["DingTalkAccessToken"];
            string DingTalkSecret = ConfigurationManager.AppSettings["DingTalkSecret"];

            if (string.IsNullOrEmpty(DingTalkAccessToken) && string.IsNullOrEmpty(DingTalkSecret))
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Console.WriteLine("首次运行请输入钉钉机器人AccessToken：");
                DingTalkAccessToken = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["DingTalkAccessToken"].Value = DingTalkAccessToken;
                Console.WriteLine("请输入钉钉机器人加签secret：");
                DingTalkSecret = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["DingTalkSecret"].Value = DingTalkSecret;
                cfa.Save();
            }
        }

        public static void SendSms(SmsContentModel smsmodel, string body)
        {
            ConfigurationManager.RefreshSection("appSettings");
            string dingTalkAccessToken = ConfigurationManager.AppSettings["DingTalkAccessToken"];
            string dingTalkSecret = ConfigurationManager.AppSettings["DingTalkSecret"];
            string url = DING_TALK_BOT_URL + dingTalkAccessToken;
            string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);

            long timestamp = ConvertDateTimeToInt(DateTime.Now);
            string sign = addSign(timestamp, dingTalkSecret);
            url += $"&timestamp={timestamp}&sign={sign}";

            JObject msgContent = new()
            {
                { "content", (string.IsNullOrEmpty(SmsCodeStr) ? "" : SmsCodeStr + "\n") + "短信转发\n" + body }
            };

            JObject msgObj = new()
            {
                { "msgtype", "text" },
                { "text", msgContent }
            };

            string resultResp = HttpHelper.Post(url, msgObj);
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

        public static string addSign(long timestamp ,string secret)
        {
            string secret1 = secret;
            string stringToSign = timestamp + "\n" + secret1;
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret1);
            byte[] messageBytes = encoding.GetBytes(stringToSign);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return HttpUtility.UrlEncode(Convert.ToBase64String(hashmessage), Encoding.UTF8);
            }
        }

        public static string Base64Encrypt(string input, Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }
        public static long ConvertDateTimeToInt(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }
    }
}
