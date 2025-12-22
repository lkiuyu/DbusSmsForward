using DbusSmsForward.SettingModel;
using System;
using System.Net;
using System.Text.Json;

namespace DbusSmsForward.Helper
{
    public static class ConfigHelper
    {

        public static void GetSettings(ref appsettingsModel result)
        {
            string settinsgFileName = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (File.Exists(settinsgFileName))
            {
                var config = File.ReadAllText(settinsgFileName);
                result = JsonSerializer.Deserialize(config, SourceGenerationContext.Default.appsettingsModel);
            }
        }

        public static void UpdateSettings(ref appsettingsModel result)
        {
            var updatedJson = JsonSerializer.Serialize(result, SourceGenerationContext.Default.appsettingsModel);
            string settinsgFileName = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            File.WriteAllText(settinsgFileName, updatedJson);
        }

        public static bool JudgeIsForwardIgnore(uint storage)
        {
            appsettingsModel result = new appsettingsModel();
            GetSettings(ref result);
            string forwardStorageType = result.appSettings.ForwardIgnoreStorageType;
            result = null;
            uint storageTypeNum = 100;
            if (forwardStorageType == "unknown" && storage == 0)
            {
                return true;
            }
            else if (forwardStorageType == "sm" && storage == 1)
            {
                return true;
            }
            else if (forwardStorageType == "me" && storage == 2)
            {
                return true;
            }
            else if (forwardStorageType == "mt" && storage == 3)
            {
                return true;
            }
            else if (forwardStorageType == "sr" && storage == 4)
            {
                return true;
            }
            else if (forwardStorageType == "bm" && storage == 5)
            {
                return true;
            }
            else if (forwardStorageType == "ta" && storage == 6)
            {
                storageTypeNum = 6;
            }
            return false;
        }

        public static string GetDeviceHostName()
        {
            try
            {
                return Dns.GetHostName();
            }
            catch
            {
                try
                {
                    return Environment.MachineName;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
    }


}
