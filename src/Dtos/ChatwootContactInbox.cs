using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Chatwoot.Dtos;

/// <summary>
/// Represents metadata about the contact's inbox.
/// </summary>
public class ChatwootContactInbox
{
    /// <summary>
    /// The unique source ID for the contact-inbox link.
    /// </summary>
    [JsonPropertyName("source_id")]
    public string SourceId { get; set; } = null!;
}