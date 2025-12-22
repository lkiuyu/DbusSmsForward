using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbusSmsForward.SettingModel
{
    public class appsettingsModel
    {
        public Appsettings appSettings { get; set; }
    }

    public class Appsettings
    {
        public Emailconfig EmailConfig { get; set; }
        public Pushplusconfig PushPlusConfig { get; set; }
        public Dingtalkconfig DingTalkConfig { get; set; }
        public Barkconfig BarkConfig { get; set; }
        public Wecomapplicationconfig WeComApplicationConfig { get; set; }
        public TGBotConfig TGBotConfig { get; set; }
        public ShellConfig ShellConfig { get; set; }
        public string SmsCodeKey { get; set; }
        public string ForwardIgnoreStorageType { get; set; }
        public string DeviceName { get; set; }
    }

    public class Emailconfig
    {
        public string smtpHost { get; set; }
        public string smtpPort { get; set; }
        public string enableSSL { get; set; }
        public string emailKey { get; set; }
        public string sendEmail { get; set; }
        public string reciveEmail { get; set; }
    }

    public class Pushplusconfig
    {
        public string pushPlusToken { get; set; }
    }

    public class Dingtalkconfig
    {
        public string DingTalkAccessToken { get; set; }
        public string DingTalkSecret { get; set; }
    }

    public class Barkconfig
    {
        public string BarkUrl { get; set; }
        public string BarkKey { get; set; }
    }

    public class Wecomapplicationconfig
    {
        public string WeChatQYID { get; set; }
        public string WeChatQYApplicationID { get; set; }
        public string WeChatQYApplicationSecret { get; set; }
    }

    public class TGBotConfig
    {
        public string IsEnableCustomTGBotApi { get; set; }
        public string CustomTGBotApi { get; set; }
        public string TGBotToken { get; set; }
        public string TGBotChatID { get; set; }
    }

    public class ShellConfig
    {
        public string ShellPath { get; set; }
    }

}
