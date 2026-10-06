using System.Text.Json;
using Soenneker.Blazor.Chatwoot;

var configuration = JsonSerializer.Deserialize("{}", LibraryJsonContext.Default.ChatwootConfiguration)!;
var payload = JsonSerializer.SerializeToElement(configuration, LibraryJsonContext.Default.ChatwootConfiguration);
Check(payload.ValueKind == JsonValueKind.Object, "configuration wire object");

Console.WriteLine("Trimmed JSON smoke checks passed.");

static void Check(bool condition, string name)
{
    if (!condition) throw new InvalidOperationException(name);
}
