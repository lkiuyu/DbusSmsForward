using DbusSmsForward.Helper;
using DbusSmsForward.SettingModel;
using DbusSmsForward.SMSModel;
using ModemManager1.DBus;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Tmds.DBus.Protocol;

namespace DbusSmsForward.ModemHelper
{
    public class ModemManagerHelper
    {
        private readonly Lock _modemObjectPathListLockObject = new();
        public Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>> _modemObjectPathList = new Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>>();
        public string _modemObjectPathNowUse = "";
        public string qmiPathNowUse = "";
        public string _modemManagerObjectPath = "/org/freedesktop/ModemManager1";
        public string? systemBusAddress = Address.System;
        public string baseService = "org.freedesktop.ModemManager1";
        public List<Action<SmsContentModel, string, string>> smsSendMethodList = new List<Action<SmsContentModel, string, string>>();
        public string deviceUidNowUse = "";
        private readonly Lock _subscribeSmsPathListLockObject = new();
        public Dictionary<string,bool> SubscribeSmsPathList=new Dictionary<string, bool>();

        public ModemManagerHelper()
        {
            if (!string.IsNullOrEmpty(systemBusAddress))
            {
                InitLoadModemObjectPathList();
                WatchModems();
            }
        }

        public void InitLoadModemObjectPathList()
        {
            lock (_modemObjectPathListLockObject)
            {
                _modemObjectPathList = GetModemsPathList().Result;
            }
            if (_modemObjectPathList.Count == 1)
            {
                SetNowUsedModemPath(_modemObjectPathList.First().Key);
            }
            if (_modemObjectPathList.Count == 0)
            {
                SetNowUsedModemPath(null);
            }
            foreach (var item in _modemObjectPathList)
            {
                SubscribeSms(item.Key);
            }
        }

        public void SetSendMethodList(List<Action<SmsContentModel, string, string>> SendMethodList)
        {
            smsSendMethodList = SendMethodList;
        }

        public void SetNowUsedModemPath(string modemObjectPathNowUse)
        {
            _modemObjectPathNowUse = modemObjectPathNowUse;
            if (string.IsNullOrEmpty(_modemObjectPathNowUse))
            {
                qmiPathNowUse = "";
                deviceUidNowUse = "";
            }
            else
            {
                qmiPathNowUse = GetQMIDevice().Result;
                deviceUidNowUse = GetDevice().Result;
            }

        }

        public void WatchModems()
        {

            Task.Run(async () =>
            {
                try
                {
                    using (var connection = new Connection(systemBusAddress))
                    {
                        await connection.ConnectAsync();
                        var service = new ModemManager1Service(connection, baseService);
                        var objectManager = service.CreateObjectManager(_modemManagerObjectPath);
                        //Console.WriteLine($"Subscribing for modem RemovedChanges");
                        await objectManager.WatchInterfacesRemovedAsync(
                        async (Exception? ex, (ObjectPath ObjectPath, string[] Interfaces) change) =>
                        {
                            if (ex is null)
                            {
                                //Console.WriteLine("modem has been removed,modem path:" + change.ObjectPath.ToString());
                                InitLoadModemObjectPathList();
                                if (!string.IsNullOrEmpty(_modemObjectPathNowUse))
                                {
                                    DisableAndEnableModem().Wait();
                                }
                            }
                        });
                        var tcs = new TaskCompletionSource<bool>();
                        var task = tcs.Task;
                        await task;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
            Task.Run(async () =>
            {
                try
                {
                    using (var connection = new Connection(systemBusAddress))
                    {
                        await connection.ConnectAsync();
                        var service = new ModemManager1Service(connection, baseService);
                        var objectManager = service.CreateObjectManager(_modemManagerObjectPath);
                        //Console.WriteLine($"Subscribing for modem AddedChanges");
                        await objectManager.WatchInterfacesAddedAsync(
                        async (Exception? ex, (ObjectPath ObjectPath, Dictionary<string, Dictionary<string, VariantValue>> InterfacesAndProperties) change) =>
                        {
                            if (ex is null)
                            {
                                //Console.WriteLine("new modem added,modem path:" + change.ObjectPath.ToString());
                                InitLoadModemObjectPathList();
                                if (!string.IsNullOrEmpty(_modemObjectPathNowUse))
                                {
                                    DisableAndEnableModem().Wait();
                                }
                            }
                        });
                        var tcs = new TaskCompletionSource<bool>();
                        var task = tcs.Task;
                        await task;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });

        }

        public void SubscribeSms(string path)
        {
            Task.Run(async () =>
            {
                try
                {
                    if ((!SubscribeSmsPathList.ContainsKey(path) || (SubscribeSmsPathList.ContainsKey(path) && !SubscribeSmsPathList[path]))&& _modemObjectPathList.ContainsKey(path))
                    {
                        if (SubscribeSmsPathList.ContainsKey(path))
                        {
                            lock (_subscribeSmsPathListLockObject)
                            {
                                SubscribeSmsPathList[path] = true;
                            }
                        }
                        else
                        {
                            lock (_subscribeSmsPathListLockObject)
                            {
                                SubscribeSmsPathList.Add(path, true);
                            }
                        }
                        //SetMsgDefaultStorageToME(path);
                        using (var connection = new Connection(systemBusAddress))
                        {
                            await connection.ConnectAsync();
                            var service = new ModemManager1Service(connection, baseService);
                            var messaging = service.CreateMessaging(path);
                            //Console.WriteLine($"add subscribe sms for modem path:" + path);
                            await messaging.WatchAddedAsync(
                            async (Exception? ex, (ObjectPath Path, bool Received) change) =>
                            {
                                if (ex is null)
                                {
                                    if (change.Received)
                                    {
                                        try
                                        {
                                            var smsPath = change.Path;
                                            var service = new ModemManager1Service(connection, baseService);
                                            var sms = service.CreateSms(smsPath);
                                            var smsState = await sms.GetStateAsync();
                                            if (smsState == 2 || smsState == 3)
                                            {
                                                uint Storage = await sms.GetStorageAsync();
                                                if (!ConfigHelper.JudgeIsForwardIgnore(Storage))
                                                {
                                                    string telNum = await sms.GetNumberAsync();
                                                    //string smsDate = (await sms.GetTimestampAsync()).Replace("T", " ").Replace("+08:00", " ");

                                                    DateTimeOffset dto = DateTimeOffset.Parse(await sms.GetTimestampAsync(), null, DateTimeStyles.RoundtripKind);
                                                    // 转换为本地时间
                                                    DateTime localTime = dto.LocalDateTime;
                                                    string smsDate = localTime.ToString("yyyy-MM-dd HH:mm:ss");
                                                    int tryCount = 0;
                                                    do
                                                    {
                                                        smsState = await sms.GetStateAsync();
                                                        Thread.Sleep(100);
                                                    } while (smsState == 2 && tryCount < 300);

                                                    if (smsState == 3)
                                                    {
                                                        appsettingsModel result = new appsettingsModel();
                                                        ConfigHelper.GetSettings(ref result);
                                                        string DeviceName = result.appSettings.DeviceName;
                                                        result = null;
                                                        if (string.IsNullOrEmpty(DeviceName) || DeviceName== "*Host*Name*")
                                                        {
                                                            DeviceName= ConfigHelper.GetDeviceHostName();
                                                        }

                                                        string smsContent = await sms.GetTextAsync();
                                                        SmsContentModel smsmodel = new SmsContentModel();
                                                        smsmodel.TelNumber = telNum;
                                                        smsmodel.SmsDate = smsDate;
                                                        smsmodel.SmsContent = smsContent;
                                                        string body = "发信电话:" + telNum + "\n" + "时间:" + smsDate + "\n" + "转发设备:" + DeviceName + "\n" + "短信内容:" + smsContent;
                                                        Console.WriteLine("smspath " + smsPath);
                                                        if (smsSendMethodList.Count() > 0)
                                                        {
                                                            //Console.WriteLine("转发方法数量"+smsSendMethodList.Count());
                                                            foreach (var action in smsSendMethodList)
                                                            {
                                                                action.Invoke(smsmodel, body, DeviceName);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("smspath:" + change.Path + " 接收超时，转发取消");
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex2)
                                        {
                                            Console.WriteLine("GetSmsError" + ex2.Message);
                                        }

                                    }
                                }
                                else
                                {
                                    Console.WriteLine(ex);
                                }
                            });
                            Task task = Task.Run(() =>
                            {
                                var modem = service.CreateModem(path);
                                
                                do
                                {
                                    try
                                    {
                                        var Manufacturer = modem.GetManufacturerAsync().Result;
                                        Manufacturer = null;
                                        Thread.Sleep(1000);
                                    }
                                    catch (Exception ex)
                                    {
                                        break;
                                    }
                                    
                                } while (SubscribeSmsPathList.ContainsKey(path) && SubscribeSmsPathList[path] && _modemObjectPathList.ContainsKey(path));
                                if (SubscribeSmsPathList.ContainsKey(path))
                                {
                                    lock (_subscribeSmsPathListLockObject)
                                    {
                                        SubscribeSmsPathList[path] = false;
                                    }
                                }
                            });
                            await task;
                            //Console.WriteLine($"remove subscribe sms for modem path:" + path);
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine($"remove subscribe sms for modem path:" + path);
                }


            });

        }

        public async Task<bool> SendSms(string telNum, string smsContent)
        {
            try
            {
                using (var connection = new Connection(Address.System))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var messaging = service.CreateMessaging(_modemObjectPathNowUse);
                    var sendsmsPath = await messaging.CreateAsync(new Dictionary<string, VariantValue> { { "text", smsContent }, { "number", telNum } });
                    var sms = service.CreateSms(sendsmsPath);
                    await sms.SendAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>>> GetModemsPathList()
        {
            try
            {
                await ScanDevices();
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var objectManager = service.CreateObjectManager(_modemManagerObjectPath);
                    return await objectManager.GetManagedObjectsAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>>();
        }

        public async Task ScanDevices()
        {
            try
            {
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modemManager = service.CreateModemManager1(_modemManagerObjectPath);
                    await modemManager.ScanDevicesAsync();
                    modemManager = null;
                    service = null;
                }
            }
            catch (Exception ex)
            {
                if (!(ex.Message.IndexOf("unsupported") >-1))
                {
                    Console.WriteLine("ScanDevices失败" + ex.Message);
                }
            }
        }

        public async Task<string> GetManufacturer(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    var Manufacturer = await modem.GetManufacturerAsync();
                    modem = null;
                    service = null;
                    return Manufacturer;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        public async Task<bool> JudgeCanGetStatus(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateSimple(actUsePath);
                    var status = await modem.GetStatusAsync();
                    modem = null;
                    service = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                return false;
            }
        }

        public async Task DisableAndEnableModem(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                if (!string.IsNullOrEmpty(actUsePath))
                {
                    using (var connection = new Connection(systemBusAddress))
                    {
                        await connection.ConnectAsync();
                        var service = new ModemManager1Service(connection, baseService);
                        var modem = service.CreateModem(actUsePath);
                        try
                        {
                            await modem.EnableAsync(false);
                            Thread.Sleep(200);
                            await modem.EnableAsync(true);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("DisableAndEnableModem:actUsePath为空");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task InhibitModemDevice(string uid, bool inhibit)
        {
            try
            {
                Console.WriteLine("device:" + uid);
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modemManager = service.CreateModemManager1(_modemManagerObjectPath);
                    await modemManager.InhibitDeviceAsync(uid, inhibit);
                    modemManager = null;
                    service = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("InhibitModemDevice失败" + ex);
            }
        }

        public async Task<string> GetDevice(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    var device = await modem.GetDeviceAsync();
                    modem = null;
                    service = null;
                    return device;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        public async Task<string> GetQMIDevice(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    var Ports = await modem.GetPortsAsync();
                    foreach (var port in Ports)
                    {
                        if (port.Item2 == 6)
                        {
                            return "/dev/" + port.Item1;
                        }
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }


        public async Task<string> GetModel(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    var Model = await modem.GetModelAsync();
                    modem = null;
                    service = null;
                    return Model;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
        public async Task<string> GetRevision(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    var Revision = await modem.GetRevisionAsync();
                    modem = null;
                    service = null;
                    return Revision;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
        public async Task<string> GetSignalQuality(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    var SignalQuality = await modem.GetSignalQualityAsync();
                    modem = null;
                    service = null;
                    return SignalQuality.Item1.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
        public async Task<string[]> GetOwnNumbers(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    var OwnNumbers = await modem.GetOwnNumbersAsync();
                    modem = null;
                    service = null;
                    return OwnNumbers;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return [];
            }
        }

        public async Task<uint?> GetPrimarySimSlot(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    var PrimarySimSlot = await modem.GetPrimarySimSlotAsync();
                    return PrimarySimSlot;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<string> GetICCID(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    var simpath = await modem.GetSimAsync();
                    var sim = service.CreateSim(simpath);
                    var iccid = await sim.GetSimIdentifierAsync();
                    modem = null;
                    service = null;
                    return iccid;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        public async Task<string> GetImei(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem3gpp = service.CreateModem3gpp(actUsePath);
                    var Imei = await modem3gpp.GetImeiAsync();
                    modem3gpp = null;
                    service = null;
                    return Imei;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
        public async Task<string> GetOperatorCode(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem3gpp = service.CreateModem3gpp(actUsePath);
                    var OperatorCode = await modem3gpp.GetOperatorCodeAsync();
                    modem3gpp = null;
                    service = null;
                    return OperatorCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
        public async Task<string> GetOperatorName(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem3gpp = service.CreateModem3gpp(actUsePath);
                    var OperatorName = await modem3gpp.GetOperatorNameAsync();
                    modem3gpp = null;
                    service = null;
                    return OperatorName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        public async Task SetMsgDefaultStorageToME(string path)
        {
            try
            {
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var messaging = service.CreateMessaging(path);
                    await messaging.SetDefaultStorageAsync(2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public bool RestartModem()
        {
            string qmiPath = GetQMIDevice().Result;
            uint? simslot = GetPrimarySimSlot().Result;
            string usedDevice = deviceUidNowUse;
            if (simslot.HasValue)
            {
                if (simslot.Value == 0)
                {
                    simslot += 1;
                }
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "qmicli",
                    Arguments = @$"-d {qmiPath} -p --uim-sim-power-off={simslot}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };
                using (Process process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();
                        if (process.ExitCode != 0)
                        {
                            Console.WriteLine(error);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to exec qmicli");
                        return false;
                    }
                }
                ProcessStartInfo startInfo2 = new ProcessStartInfo
                {
                    FileName = "qmicli",
                    Arguments = @$"-d {qmiPath} -p --uim-sim-power-on={simslot}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };
                using (Process process = Process.Start(startInfo2))
                {
                    if (process != null)
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();
                        if (process.ExitCode != 0)
                        {
                            Console.WriteLine(error);
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to exec qmicli");
                        return false;
                    }
                }
                if (JudgeCanGetStatus().Result)
                {
                    DisableAndEnableModem().Wait();
                }
                Thread.Sleep(200);
                if (JudgeCanGetStatus().Result)
                {
                    InhibitModemDevice(usedDevice, true).Wait();
                    Thread.Sleep(200);
                    InhibitModemDevice(usedDevice, false).Wait();
                }

                return true;

            }
            else
            {
                return false;
            }
        }

    }

}
