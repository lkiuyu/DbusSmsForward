using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DbusSmsForward
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(JsonObject))]
    internal partial class SourceGenerationContext : JsonSerializerContext
    {
    }
}
