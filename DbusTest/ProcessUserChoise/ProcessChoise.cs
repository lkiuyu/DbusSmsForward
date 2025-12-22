using DbusSmsForward.SendMethod;
using DbusSmsForward.SMSModel;
using System.Collections.Generic;

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

        public static List<Action<SmsContentModel, string, string>> sendMethodGuide(List<string> chooseOptions)
        {
            if(chooseOptions.Count() == 0)
            {
                Console.WriteLine("请选择转发渠道：1.邮箱转发，2.pushplus转发，3.企业微信转发，4.TG机器人转发，5.钉钉转发，6.Bark转发，7.自定义shell转发，同时转发多渠道请以空格分割编号（举例：1 2 3 5）");
                chooseOptions.Add(Console.ReadLine());
                return sendMethodGuide(chooseOptions);
            }
            else if (chooseOptions.Count() >= 1)
            {
                
                if (chooseOptions.Count() == 1 && chooseOptions[0].IndexOf(" ") > -1)
                {
                    string chooseOption = chooseOptions[0];
                    List<string> newChooseOptions = chooseOption.Split(" ").ToList();
                    return sendMethodGuide(newChooseOptions);
                }
                else
                {
                    if (JudgeChooseIsValid(chooseOptions,true))
                    {
                        List<Action<SmsContentModel, string, string>> actions = new List<Action<SmsContentModel, string, string>>();
                        foreach (var chooseOption in chooseOptions)
                        {
                            if (chooseOption == "1")
                            {
                                SendByEmail.SetupEmailInfo();
                                actions.Add(SendByEmail.SendSms);
                            }
                            if (chooseOption == "2")
                            {
                                SendByPushPlus.SetupPushPlusInfo();
                                actions.Add(SendByPushPlus.SendSms);
                            }
                            if (chooseOption == "3")
                            {
                                SendByWeComApplication.SetupWeComInfo();
                                actions.Add(SendByWeComApplication.SendSms);
                            }
                            if (chooseOption == "4")
                            {
                                SendByTelegramBot.SetupTGBotInfo();
                                actions.Add(SendByTelegramBot.SendSms);
                            }
                            if (chooseOption == "5")
                            {
                                SendByDingTalkBot.SetupDingtalkBotMsg();
                                actions.Add(SendByDingTalkBot.SendSms);
                            }
                            if (chooseOption == "6")
                            {
                                SendByBark.SetupBarkInfo();
                                actions.Add(SendByBark.SendSms);
                            }
                            if (chooseOption == "7")
                            {
                                SendByShell.SetupCustomShellInfo();
                                actions.Add(SendByShell.SendSms);
                            }
                        }
                        return actions;
                    }
                    else
                    {
                        Console.WriteLine("请输入1或2或3或4或5或6或7");
                        return sendMethodGuide(new List<string>());
                    }
                }
            }
            return new List<Action<SmsContentModel, string, string>>();
        }
        public static bool JudgeChooseIsValid(List<string> chooseOptions,bool isCheckAll=false)
        {
            if (isCheckAll)
            {
                return chooseOptions.Where(a => a == "1" || a == "2" || a == "3" || a == "4" || a == "5" || a == "6" || a == "7").Count() == chooseOptions.Count();
            }
            else
            {
                return chooseOptions.Where(a => a == "1" || a == "2" || a == "3" || a == "4" || a == "5" || a == "6" || a == "7").Count() > 0;
            }
        }

    }
}
