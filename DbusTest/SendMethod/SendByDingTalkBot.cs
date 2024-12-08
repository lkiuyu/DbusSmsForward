using DbusSmsForward.Helper;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using DbusSmsForward.SMSModel;
using DbusSmsForward.ProcessSmsContent;
using System.Text.Json;
using System.Text.Json.Nodes;
using DbusSmsForward.SettingModel;

namespace DbusSmsForward.SendMethod
{
    public class SendByDingTalkBot
    {
        private const string DING_TALK_BOT_URL = "https://oapi.dingtalk.com/robot/send?access_token=";

        public static void SetupDingtalkBotMsg()
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string DingTalkAccessToken = result.appSettings.DingTalkConfig.DingTalkAccessToken;
            string DingTalkSecret = result.appSettings.DingTalkConfig.DingTalkSecret;

            if (string.IsNullOrEmpty(DingTalkAccessToken) && string.IsNullOrEmpty(DingTalkSecret))
            {
                Console.WriteLine("首次运行请输入钉钉机器人AccessToken：");
                DingTalkAccessToken = Console.ReadLine().Trim();
                result.appSettings.DingTalkConfig.DingTalkAccessToken = DingTalkAccessToken;
                Console.WriteLine("请输入钉钉机器人加签secret：");
                DingTalkSecret = Console.ReadLine().Trim();
                result.appSettings.DingTalkConfig.DingTalkSecret = DingTalkSecret;
                ConfigHelper.UpdateSettings(ref result);
            }
            result = null;
        }

        public static void SendSms(SmsContentModel smsmodel, string body)
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string dingTalkAccessToken = result.appSettings.DingTalkConfig.DingTalkAccessToken;
            string dingTalkSecret = result.appSettings.DingTalkConfig.DingTalkSecret;
            result = null;
            string url = DING_TALK_BOT_URL + dingTalkAccessToken;
            string SmsCodeStr = GetSmsContentCode.GetSmsCodeStr(smsmodel.SmsContent);

            long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            string sign = addSign(timestamp, dingTalkSecret);
            url += $"&timestamp={timestamp}&sign={sign}";

            JsonObject msgContent = new()
            {
                { "content", (string.IsNullOrEmpty(SmsCodeStr) ? "" : SmsCodeStr + "\n") + "短信转发\n" + body }
            };

            JsonObject msgObj = new()
            {
                { "msgtype", "text" },
                { "text", msgContent }
            };

            string resultResp = HttpHelper.Post(url, msgObj);
            //JObject jsonObjresult = JObject.Parse(resultResp);
            JsonObject jsonObjresult = JsonSerializer.Deserialize(resultResp, SourceGenerationContext.Default.JsonObject);
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
    }
}
