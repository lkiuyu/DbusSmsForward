using DbusSmsForward.SettingModel;
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
                result= JsonSerializer.Deserialize(config, SourceGenerationContext.Default.appsettingsModel);
            }
        }

        public static void UpdateSettings(ref appsettingsModel result)
        {
            var updatedJson = JsonSerializer.Serialize(result, SourceGenerationContext.Default.appsettingsModel);
            string settinsgFileName = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            File.WriteAllText(settinsgFileName, updatedJson);
        }
    }

    
}
