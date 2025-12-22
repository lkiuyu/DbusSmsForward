using DbusSmsForward.Helper;
using DbusSmsForward.ProcessSmsContent;
using DbusSmsForward.SettingModel;
using DbusSmsForward.SMSModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

namespace DbusSmsForward.SendMethod
{
    public static class SendByShell
    {
        public static void SetupCustomShellInfo()
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string shellPath = result.appSettings.ShellConfig.ShellPath;
            if (string.IsNullOrEmpty(shellPath))
            {
                Console.WriteLine("首次运行请输入自定义shell脚本路径：");
                shellPath = Console.ReadLine().Trim();
                result.appSettings.ShellConfig.ShellPath = shellPath;
                ConfigHelper.UpdateSettings(ref result);
            }
            result = null;
        }

        public static void SendSms(SmsContentModel smsmodel, string body, string devicename)
        {
            appsettingsModel result = new appsettingsModel();
            ConfigHelper.GetSettings(ref result);
            string shellPath = result.appSettings.ShellConfig.ShellPath;
            result = null;

            SmsCodeModel codeResult = GetSmsContentCode.GetSmsCodeModel(smsmodel.SmsContent);
            string[] arguments =
            {
                smsmodel.TelNumber,
                smsmodel.SmsDate,
                smsmodel.SmsContent,
                codeResult.CodeValue,
                codeResult.CodeFrom,
                devicename
            };

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = shellPath,
                UseShellExecute = false,  // 不直接使用操作系统shell启动
                CreateNoWindow = true,     // 不创建新窗口
                RedirectStandardOutput = true,  // 可以重定向输出
                RedirectStandardError = true    // 可以重定向错误
            };
            foreach (var arg in arguments)
            {
                startInfo.ArgumentList.Add(arg);
            }

            // 启动进程
            using (Process process = new Process())
            {
                process.StartInfo = startInfo;

                // 可以捕获输出（如果需要）
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"输出: {e.Data}");
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                        Console.WriteLine($"错误: {e.Data}");
                };

                bool started = process.Start();

                if (started)
                {
                    // 开始异步读取输出
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // 等待进程完成（可以设置超时时间）
                    bool exited = process.WaitForExit(30000); // 30秒超时

                    if (exited)
                    {
                        int exitCode = process.ExitCode;

                        if (exitCode == 0)
                        {
                            Console.WriteLine("\nShell调用成功");
                        }
                        else
                        {
                            Console.WriteLine($"\nShell调用失败，退出代码: {exitCode}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nShell调用超时");
                        process.Kill(); // 强制终止进程
                    }
                }
                else
                {
                    Console.WriteLine("\n无法启动Shell进程");
                }
            }
        }
    }
}
