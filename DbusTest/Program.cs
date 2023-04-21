// See https://aka.ms/new-console-template for more information
using ModemManager1.DBus;
using System.Configuration;
using Tmds.DBus;
using System.Net.Mail;
using System.Net;
using DbusSmsForward.SendMethod;

string smtpHost = ConfigurationManager.AppSettings["smtpHost"];
string smtpPort = ConfigurationManager.AppSettings["smtpPort"];
string emailKey = ConfigurationManager.AppSettings["emailKey"];
string sendEmial = ConfigurationManager.AppSettings["sendEmial"];
string reciveEmial = ConfigurationManager.AppSettings["reciveEmial"];
string pushPlusToken = ConfigurationManager.AppSettings["pushPlusToken"];


string StartGuideResult = onStartGuide();
if (StartGuideResult == "1")
{
    string sendMethodGuideResult = sendMethodGuide();
    if (sendMethodGuideResult=="1")
    {
        if (string.IsNullOrEmpty(smtpHost) && string.IsNullOrEmpty(smtpPort) && string.IsNullOrEmpty(emailKey) && string.IsNullOrEmpty(sendEmial) && string.IsNullOrEmpty(reciveEmial))
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            Console.WriteLine("首次运行请输入邮箱转发相关配置信息\n请输入smtp地址：");
            smtpHost = Console.ReadLine().Trim();
            cfa.AppSettings.Settings["smtpHost"].Value = smtpHost;

            Console.WriteLine("请输入smtp端口：");
            smtpPort = Console.ReadLine().Trim();
            cfa.AppSettings.Settings["smtpPort"].Value = smtpPort;

            Console.WriteLine("请输入邮箱密钥：");
            emailKey = Console.ReadLine().Trim();
            cfa.AppSettings.Settings["emailKey"].Value = emailKey;

            Console.WriteLine("请输入发件邮箱：");
            sendEmial = Console.ReadLine().Trim();
            cfa.AppSettings.Settings["sendEmial"].Value = sendEmial;

            Console.WriteLine("请输入收件邮箱：");
            reciveEmial = Console.ReadLine().Trim();
            cfa.AppSettings.Settings["reciveEmial"].Value = reciveEmial;

            cfa.Save();

        }
    }
    else if (string.IsNullOrEmpty(pushPlusToken))
    {
        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        Console.WriteLine("首次运行请输入PushPlusToken：");
        pushPlusToken = Console.ReadLine().Trim();
        cfa.AppSettings.Settings["pushPlusToken"].Value = pushPlusToken;
        cfa.Save();
    }


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
                     string smscontent = await isms.GetTextAsync();
                     string body = "发信电话:" + tel + "\n" + "时间:" + stime + "\n" + "短信内容:" + smscontent;
                     Console.WriteLine(body);
                     if (sendMethodGuideResult == "1")
                     {
                         SendByEmail.SendSms(tel, body);
                     }
                     
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

string onStartGuide()
{
    Console.WriteLine("请选择运行模式：1为短信转发模式，2为发短信模式");
    string chooseOption=Console.ReadLine();
    if(chooseOption =="1"|| chooseOption == "2")
    {
        if (chooseOption == "1")
        {
            return "1";
        }
        else if(chooseOption == "2")
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
        return onStartGuide();
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


string sendMethodGuide()
{
    Console.WriteLine("请选择转发渠道：1.邮箱转发，2.pushplus");
    string chooseOption = Console.ReadLine();
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
        return sendMethodGuide();
    }

}
