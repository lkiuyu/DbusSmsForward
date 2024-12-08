using DbusSmsForward.ProcessUserChoise;
using DbusSmsForward.SMSModel;
using DbusSmsForward.ModemHelper;


ModemManagerHelper.SetModemObjectPathList();
ModemManagerHelper.WatchModems();
string startGuideChoiseNum = "";
List<string> sendMethodGuideChoiseNumArray= new List<string>();
foreach (var s1 in args)
{
    if (s1 == "-fE")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNumArray.Add("1");
    }
    else if (s1 == "-fP")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNumArray.Add("2");
    }
    else if (s1 == "-fW")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNumArray.Add("3");
    }
    else if (s1 == "-fT")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNumArray.Add("4");
    }
    else if (s1 == "-fD")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNumArray.Add("5");
    }
    else if (s1 == "-fB")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNumArray.Add("6");
    }
    else if (s1 == "-sS")
    {
        startGuideChoiseNum = "2";
    }
}
string StartGuideResult = ProcessChoise.onStartGuide(startGuideChoiseNum);
if (StartGuideResult == "1")
{
    List<Action<SmsContentModel, string>> actionList=  ProcessChoise.sendMethodGuide(sendMethodGuideChoiseNumArray);
    ModemManagerHelper.SetSendMethodList(actionList);
    Console.WriteLine("正在运行. 按下 Ctrl-C 停止.");
    var tcs = new TaskCompletionSource<bool>();
    var task = tcs.Task;
    await task;

}
else if(StartGuideResult == "2")
{
    string telNumber = string.Empty, smsText=string.Empty;
    sendSms(ref telNumber,ref smsText);
    Console.WriteLine("短信创建成功，是否发送？(1.发送短信,其他按键退出程序)");
    string sendChoise = Console.ReadLine();
    if (sendChoise == "1")
    {
        if (ModemManagerHelper.SendSms(telNumber, smsText).Result)
        {
            Console.WriteLine("短信已发送");
        }
        else
        {
            Console.WriteLine("短信发送失败");
        }
    }
}



void sendSms(ref string telNumber,ref string smsText)
{
    Console.WriteLine("请输入收信号码：");
    telNumber = Console.ReadLine();
    Console.WriteLine("请输入短信内容");
    smsText = Console.ReadLine();

}



