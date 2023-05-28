using DbusSmsForward.SendMethod;

namespace DbusSmsForward.ProcessUserChoise
{
    public static class ProcessChoise
    {
        public static string onStartGuide(string chooseOption)
        {
            if (string.IsNullOrEmpty(chooseOption))
            {
                Console.WriteLine("请选择运行模式：1为短信转发模式，2为发短信模式");
                chooseOption = Console.ReadLine();
            }
            if (chooseOption == "1" || chooseOption == "2")
            {
                return chooseOption;
            }
            else
            {
                Console.WriteLine("请输入1或2");
                return onStartGuide("");
            }

        }

        public static string sendMethodGuide(string chooseOption)
        {
            if(string.IsNullOrEmpty(chooseOption))
            {
                Console.WriteLine("请选择转发渠道：1.邮箱转发，2.pushplus转发，3.企业微信转发，4.TG机器人转发，5.钉钉转发");
                chooseOption = Console.ReadLine();
            }
            if (chooseOption == "1" || chooseOption == "2" || chooseOption == "3" || chooseOption == "4" || chooseOption == "5")
            {
                if (chooseOption == "1")
                {
                    SendByEmail.SetupEmailInfo();
                    return "1";
                }
                else if (chooseOption == "2")
                {
                    SendByPushPlus.SetupPushPlusInfo();
                    return "2";
                }
                else if (chooseOption == "3")
                {
                    SendByWeComApplication.SetupWeComInfo();
                    return "3";
                }
                else if (chooseOption == "4")
                {
                    SendByTelegramBot.SetupTGBotInfo();
                    return "4";
                }else if(chooseOption == "5")
                {
                    SendByDingTalkBot.SetupDingtalkBotMsg();
                    return "5";
                }
                else
                {
                    return "";
                }
            }
            else
            {
                Console.WriteLine("请输入1或2或3或4或5");
                return sendMethodGuide("");
            }
        }

    }
}
