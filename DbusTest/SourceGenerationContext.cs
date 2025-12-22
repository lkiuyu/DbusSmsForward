using DbusSmsForward.SettingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DbusSmsForward
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(JsonObject))]
    [JsonSerializable(typeof(appsettingsModel))]
    internal partial class SourceGenerationContext : JsonSerializerContext
    {
        static SourceGenerationContext() => Default = new SourceGenerationContext(CreateJsonSerializerOptions(Default));
        private static JsonSerializerOptions CreateJsonSerializerOptions(SourceGenerationContext context)
        {
            JsonSerializerOptions options = new(context.GeneratedSerializerOptions!)
            {
                PropertyNamingPolicy = null,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            return options;
        }
    }
}
