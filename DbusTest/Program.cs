using DbusSmsForward.Helper;
using DbusSmsForward.ModemHelper;
using DbusSmsForward.ProcessUserChoise;
using DbusSmsForward.SettingModel;
using DbusSmsForward.SMSModel;


ModemManagerHelper mmhelper=new ModemManagerHelper();
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
    else if (s1 == "-fS")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNumArray.Add("7");
    }
    else if (s1 == "-sS")
    {
        startGuideChoiseNum = "2";
    }
}
string StartGuideResult = ProcessChoise.onStartGuide(startGuideChoiseNum);
if (StartGuideResult == "1")
{
    appsettingsModel result = new appsettingsModel();
    ConfigHelper.GetSettings(ref result);
    string DeviceName = result.appSettings.DeviceName;
    
    if (string.IsNullOrEmpty(DeviceName))
    {
        Console.WriteLine("初次运行是否需要设置转发设备名称?(留空回车则默认动态读取设备主机名)：");
        DeviceName = Console.ReadLine().Trim();
        if (string.IsNullOrEmpty(DeviceName))
        {
            result.appSettings.DeviceName = "*Host*Name*";
        }
        else
        {
            result.appSettings.DeviceName = DeviceName;
        }
    }
    ConfigHelper.UpdateSettings(ref result);
    result = null;

    List<Action<SmsContentModel, string, string>> actionList=  ProcessChoise.sendMethodGuide(sendMethodGuideChoiseNumArray);
    mmhelper.SetSendMethodList(actionList);
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
        if (mmhelper.SendSms(telNumber, smsText).Result)
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



