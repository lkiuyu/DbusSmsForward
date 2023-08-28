using ModemManager1.DBus;
using Tmds.DBus;
using DbusSmsForward.SendMethod;
using DbusSmsForward.ProcessUserChoise;
using DbusSmsForward.ProcessUserSend;

string startGuideChoiseNum = "";
string sendMethodGuideChoiseNum = "";

foreach (var s1 in args)
{
    if (s1 == "-fE")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNum = "1";
    }
    else if (s1 == "-fP")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNum = "2";
    }
    else if (s1 == "-fW")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNum = "3";
    }
    else if (s1 == "-fT")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNum = "4";
    }
    else if (s1 == "-fD")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNum = "5";
    }
    else if (s1 == "-fB")
    {
        startGuideChoiseNum = "1";
        sendMethodGuideChoiseNum = "6";
    }
    else if (s1 == "-sS")
    {
        startGuideChoiseNum = "2";
    }
    break;
}
string StartGuideResult = ProcessChoise.onStartGuide(startGuideChoiseNum);
if (StartGuideResult == "1")
{
    string sendMethodGuideResult = ProcessChoise.sendMethodGuide(sendMethodGuideChoiseNum);
    Console.WriteLine("正在运行. 按下 Ctrl-C 停止.");

    Task.Run(async () =>
    {
        using (var connection = new Connection(Address.System))
        {
            await connection.ConnectAsync();
            var objectPath = new ObjectPath("/org/freedesktop/ModemManager1/Modem/0");
            var service = "org.freedesktop.ModemManager1";
            var imsg = connection.CreateProxy<IMessaging>(service, objectPath);
            await imsg.WatchAddedAsync(
             async change =>
             {
                 if (change.received)
                 {
                     Console.WriteLine(change.path);
                     var isms = connection.CreateProxy<ISms>("org.freedesktop.ModemManager1", change.path);
                     string tel = await isms.GetNumberAsync();
                     string stime = (await isms.GetTimestampAsync()).Replace("T", " ").Replace("+08:00", " ");
                     string smscontent = "";
                     do
                     {
                         smscontent = "";
                         smscontent = await isms.GetTextAsync();
                     } while (string.IsNullOrEmpty(smscontent));
                     string body = "发信电话:" + tel + "\n" + "时间:" + stime + "\n" + "短信内容:" + smscontent;
                     Console.WriteLine(body);
                     ProcessSend.sendSms(sendMethodGuideResult, tel, body);
                 }
             }
         );
            await Task.Delay(int.MaxValue);
        }
    }).Wait();
}
else if(StartGuideResult == "2")
{
    using (var connection = new Connection(Address.System))
    {
        await connection.ConnectAsync();
        var objectPath = new ObjectPath("/org/freedesktop/ModemManager1/Modem/0");
        var service = "org.freedesktop.ModemManager1";
        var imsg = connection.CreateProxy<IMessaging>(service, objectPath);
        var sendsmsPath=await imsg.CreateAsync(sendSms());
        Console.WriteLine("短信创建成功，是否发送？(1.发送短信,其他按键退出程序)");
        string sendChoise=Console.ReadLine();
        if(sendChoise == "1")
        {
            var isms = connection.CreateProxy<ISms>("org.freedesktop.ModemManager1", sendsmsPath);
            await isms.SendAsync();
            Console.WriteLine("短信已发送");
        }
        else
        {
            await imsg.DeleteAsync(sendsmsPath);
            Console.WriteLine("短信缓存已清理，按回车返回运行模式选择");
            Console.ReadLine();
            
        }
    }
}



Dictionary<string, object> sendSms()
{
    Console.WriteLine("请输入收信号码：");
    string telNumber = Console.ReadLine();
    Console.WriteLine("请输入短信内容");
    string smsText = Console.ReadLine();
    return new Dictionary<string, object> { { "text", smsText }, { "number", telNumber } };

}



