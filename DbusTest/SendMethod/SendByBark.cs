using DbusSmsForward.Helper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbusSmsForward.SendMethod
{
    public static class SendByBark
    {
        public static void SetupBarkInfo()
        {
            string BarkUrl = ConfigurationManager.AppSettings["BarkUrl"];
            string BrakKey = ConfigurationManager.AppSettings["BrakKey"];

            if (string.IsNullOrEmpty(BarkUrl) && string.IsNullOrEmpty(BrakKey))
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Console.WriteLine("首次运行请输入Bark服务器地址：");
                BarkUrl = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["BarkUrl"].Value = BarkUrl;
                Console.WriteLine("首次运行请输入Bark推送key");
                BrakKey = Console.ReadLine().Trim();
                cfa.AppSettings.Settings["BrakKey"].Value = BrakKey;
                cfa.Save();
            }
        }

        public static void SendSms(string number, string body)
        {
            try
            {
                ConfigurationManager.RefreshSection("appSettings");
                string BarkUrl = ConfigurationManager.AppSettings["BarkUrl"];
                string BrakKey = ConfigurationManager.AppSettings["BrakKey"];
                string url = BarkUrl + "/" + BrakKey + "/";
                url += System.Web.HttpUtility.UrlEncode(body);
                url += "?group=" + number + "&title=短信转发" + number;
                string msgresult = HttpHelper.HttpGet(url);
                Console.WriteLine("Bark转发成功");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }
}
