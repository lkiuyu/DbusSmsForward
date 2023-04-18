// See https://aka.ms/new-console-template for more information
using ModemManager1.DBus;
using System.Configuration;
using Tmds.DBus;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Net;

string smtpHost = ConfigurationManager.AppSettings["smtpHost"];
string smtpPort = ConfigurationManager.AppSettings["smtpPort"];
string emailKey = ConfigurationManager.AppSettings["emailKey"];
string sendEmial = ConfigurationManager.AppSettings["sendEmial"];
string reciveEmial = ConfigurationManager.AppSettings["reciveEmial"];

if (string.IsNullOrEmpty(smtpHost)&& string.IsNullOrEmpty(smtpPort) && string.IsNullOrEmpty(emailKey) && string.IsNullOrEmpty(sendEmial) && string.IsNullOrEmpty(reciveEmial))
{
    Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
    Console.WriteLine("首次运行请输入邮箱转发相关配置信息\n请输入smtp地址：");
    smtpHost=Console.ReadLine().Trim();
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

    Console.WriteLine("正在运行. 按下 Ctrl-C 停止.");

}
else
{
    Console.WriteLine("正在运行. 按下 Ctrl-C 停止.");
}

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
             Console.WriteLine(change.path);
             var isms = connection.CreateProxy<ISms>("org.freedesktop.ModemManager1", change.path);
             string tel = await isms.GetNumberAsync();
             string stime = (await isms.GetTimestampAsync()).Replace("T", " ").Replace("+08:00", " ");
             string smscontent = await isms.GetTextAsync();
             string body = "发信电话:" + tel + "\n" + "时间:" + stime + "\n" + "短信内容:" + smscontent;
             Console.WriteLine(body);
             MailAddress to = new MailAddress(reciveEmial);
             MailAddress from = new MailAddress(sendEmial);
             MailMessage mm = new MailMessage(from, to);
             SmtpClient sc = new SmtpClient(smtpHost);
             try
             {
                 mm.Subject = "短信转发" + tel;
                 mm.Body = body;
                 sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                 sc.Credentials = new NetworkCredential(sendEmial, emailKey);
                 sc.Send(mm);
                 Console.WriteLine("转发成功");
                 mm.Dispose();
                 sc.Dispose();
             }
             catch (SmtpException ex)
             {
                 mm.Dispose();
                 sc.Dispose();
                 Console.WriteLine(ex);
                 Console.WriteLine("出错了，尝试确认下配置文件中的邮箱信息是否正确，配置文件为DbusSmsFoward.dll.config");
             }
         }
     );
        await Task.Delay(int.MaxValue);

    }
}).Wait();
Console.WriteLine("end");


