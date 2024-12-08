using DbusSmsForward.SMSModel;
using ModemManager1.DBus;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Tmds.DBus.Protocol;

namespace DbusSmsForward.ModemHelper
{
    public static class ModemManagerHelper
    {
        public static Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>> _modemObjectPathList = new Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>>();
        public static string _modemObjectPathNowUse = "";
        public static string qmiPathNowUse = "";
        public static string _modemManagerObjectPath = "/org/freedesktop/ModemManager1";
        public static string? systemBusAddress = Address.System;
        public static string baseService = "org.freedesktop.ModemManager1";
        public static bool isSetModemObjectPathListDone = false;
        public static List<Action<SmsContentModel, string>> smsSendMethodList = new List<Action<SmsContentModel, string>>();

        public static async void SetModemObjectPathList()
        {
            if (!isSetModemObjectPathListDone)
            {
                _modemObjectPathList = await GetModemsPathList();
                isSetModemObjectPathListDone = true;
                if (_modemObjectPathList.Count == 1)
                {
                    SetNowUsedModemPath(_modemObjectPathList.First().Key);
                }
                foreach (var item in _modemObjectPathList)
                {
                    SubscribeSms(item.Key);
                }
            }
        }
        
        public static void SetNowUsedModemPath(string modemObjectPathNowUse)
        {
            _modemObjectPathNowUse = modemObjectPathNowUse;
            if (string.IsNullOrEmpty(_modemObjectPathNowUse))
            {
                qmiPathNowUse = "";
            }
            else
            {
                qmiPathNowUse = GetQMIDevice().Result;
            }
        }
        public static void SetSendMethodList(List<Action<SmsContentModel, string>> SendMethodList)
        {
            smsSendMethodList = SendMethodList;
        }


        public static bool JudgeNowModemIsAvaliable(ref int statusCode,ref string errorMsg)
        {
            if (!string.IsNullOrEmpty(_modemObjectPathNowUse)&& _modemObjectPathList.Count==1)
            {
                return true;
            }
            else if (!string.IsNullOrEmpty(_modemObjectPathNowUse) && _modemObjectPathList.Count > 1)
            {
                statusCode = 1;
                return true;
            }
            else if (_modemObjectPathList.Count>1)
            {
                statusCode = 2;
                errorMsg = "搜索到有多个可用modem，请选择一个默认使用";
                return false;
            }
            else
            {
                statusCode = 3;
                errorMsg = "未找到可用的modem";
                return false;
            }
        }
        
        public static void WatchModems()
        {
            try
            {
                Task.Run(async () =>
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
                                if (change.ObjectPath == _modemObjectPathNowUse)
                                {
                                    Console.WriteLine("The modem currently in use has been removed,you may need select new modem,removed modem path:" + change.ObjectPath.ToString());
                                    _modemObjectPathList.Remove(change.ObjectPath);
                                    SetNowUsedModemPath(string.Empty);
                                }
                                else
                                {
                                    Console.WriteLine("modem has been removed,modem path:" + change.ObjectPath.ToString());
                                }
                            }
                        });
                        var tcs = new TaskCompletionSource<bool>();
                        var task = tcs.Task;
                        await task;
                    }
                });
                Task.Run(async () =>
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
                                if (!_modemObjectPathList.ContainsKey(change.ObjectPath))
                                {
                                    _modemObjectPathList.Add(change.ObjectPath, change.InterfacesAndProperties);
                                    Console.WriteLine("new modem added,modem path:" + change.ObjectPath.ToString());
                                    if (string.IsNullOrEmpty(_modemObjectPathNowUse) && _modemObjectPathList.Count == 1)
                                    {
                                        SetNowUsedModemPath(change.ObjectPath);
                                    }
                                    SubscribeSms(change.ObjectPath);
                                }
                            }
                        });
                        var tcs = new TaskCompletionSource<bool>();
                        var task = tcs.Task;
                        await task;
                    }
                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void SubscribeSms(string path)
        {
            try
            {
                Task.Run(async () =>
                {
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
                                    var smsPath = change.Path;
                                    var service = new ModemManager1Service(connection, baseService);
                                    var sms = service.CreateSms(smsPath);
                                    var smsState = await sms.GetStateAsync();
                                    if (smsState==2|| smsState==3)
                                    {
                                        string telNum = await sms.GetNumberAsync();
                                        string smsDate = (await sms.GetTimestampAsync()).Replace("T", " ").Replace("+08:00", " ");
                                        do
                                        {
                                            smsState = await sms.GetStateAsync();
                                            Thread.Sleep(100);
                                        } while (smsState == 2);
                                        string smsContent = await sms.GetTextAsync();
                                        SmsContentModel smsmodel = new SmsContentModel();
                                        smsmodel.TelNumber = telNum;
                                        smsmodel.SmsDate = smsDate;
                                        smsmodel.SmsContent = smsContent;
                                        string body = "发信电话:" + telNum + "\n" + "时间:" + smsDate + "\n" + "短信内容:" + smsContent;
                                        if (smsSendMethodList.Count()>0)
                                        {
                                            foreach (var action in smsSendMethodList)
                                            {
                                                action.Invoke(smsmodel, body);
                                            }
                                        }
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
                            do
                            {
                                Thread.Sleep(100);
                            } while (_modemObjectPathList.ContainsKey(path));
                        });
                        await task;
                        Console.WriteLine($"remove subscribe sms for modem path:" + path);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static async Task<bool> SendSms(string telNum,string smsContent)
        {
            try
            {
                using (var connection = new Connection(Address.System))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var messaging = service.CreateMessaging(_modemObjectPathNowUse);
                    var sendsmsPath = await messaging.CreateAsync(new Dictionary<string, Variant> { { "text", smsContent }, { "number", telNum } });
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

        public static async Task<Dictionary<ObjectPath, Dictionary<string, Dictionary<string, VariantValue>>>> GetModemsPathList()
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

        public static async Task ScanDevices()
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
                Console.WriteLine(ex);
            }
        }

        public static async Task<string> GetManufacturer(string? path=null)
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
            catch (Exception ex) {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        public static async Task DisableModem(string? path = null)
        {
            try
            {
                string actUsePath = path == null ? _modemObjectPathNowUse : path;
                using (var connection = new Connection(systemBusAddress))
                {
                    await connection.ConnectAsync();
                    var service = new ModemManager1Service(connection, baseService);
                    var modem = service.CreateModem(actUsePath);
                    await modem.EnableAsync(false);
                    try
                    {
                        await modem.EnableAsync(true);
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static async Task<string> GetQMIDevice(string? path = null)
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
                        if (port.Item2==6)
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

        public static async Task<string> GetModel(string? path = null)
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
        
        public static async Task<string> GetRevision(string? path = null)
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
        
        public static async Task<string> GetSignalQuality(string? path = null)
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
                    modem= null;
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
        
        public static async Task<string[]> GetOwnNumbers(string? path = null)
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

        public static async Task<uint?> GetPrimarySimSlot(string? path = null)
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

        public static async Task<string> GetICCID(string? path = null)
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
                    var iccid= await sim.GetSimIdentifierAsync();
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

        public static async Task<string> GetImei(string? path = null)
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
        
        public static async Task<string> GetOperatorCode(string? path = null)
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
        
        public static async Task<string> GetOperatorName(string? path = null)
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
                    service=null;
                    return OperatorName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }

        public static bool RestartModem()
        {
            string qmiPath = GetQMIDevice().Result;
            uint? simslot=GetPrimarySimSlot().Result;
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
                        string error=process.StandardError.ReadToEnd();
                        process.WaitForExit();
                        if (process.ExitCode!=0)
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
                DisableModem().Wait();
                return true;

            }
            else
            {
                return false;
            }
        }

    }

}
