using Soenneker.Blazor.Chatwoot.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using Soenneker.Blazor.Chatwoot.Dtos;

namespace Soenneker.Blazor.Chatwoot;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, ReadCommentHandling = JsonCommentHandling.Skip, UseStringEnumConverter = true)]
[JsonSerializable(typeof(ChatwootMessage))]
[JsonSerializable(typeof(ChatwootConfiguration))]
internal partial class LibraryJsonContext : JsonSerializerContext
{
}
