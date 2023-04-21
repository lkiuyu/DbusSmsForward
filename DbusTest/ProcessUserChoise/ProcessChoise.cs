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
                if (chooseOption == "1")
                {
                    return "1";
                }
                else if (chooseOption == "2")
                {
                    return "2";
                }
                else
                {
                    return "";
                }
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
                Console.WriteLine("请选择转发渠道：1.邮箱转发，2.pushplus转发，3.企业微信转发");
                chooseOption = Console.ReadLine();
            }
            if (chooseOption == "1" || chooseOption == "2" || chooseOption == "3")
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
                else
                {
                    return "";
                }
            }
            else
            {
                Console.WriteLine("请输入1或2或3");
                return sendMethodGuide("");
            }
        }

    }
}
