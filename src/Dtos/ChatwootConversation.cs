using System;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Chatwoot.Dtos;

/// <summary>
/// Represents conversation details associated with a message.
/// </summary>
public sealed class ChatwootConversation
{
    /// <summary>
    /// The ID of the user assigned to the conversation (nullable).
    /// </summary>
    [JsonPropertyName("assignee_id")]
    public int? AssigneeId { get; set; }

    /// <summary>
    /// The number of unread messages in the conversation.
    /// </summary>
    [JsonPropertyName("unread_count")]
    public int UnreadCount { get; set; }

    /// <summary>
    /// The UNIX timestamp of the last activity.
    /// </summary>
    [JsonPropertyName("last_activity_at")]
    public long LastActivityAtUnix { get; set; }

    /// <summary>
    /// The UTC DateTime representation of the last activity.
    /// </summary>
    [JsonIgnore]
    public DateTime LastActivityAt => DateTimeOffset.FromUnixTimeSeconds(LastActivityAtUnix).UtcDateTime;

    /// <summary>
    /// The contact inbox metadata.
    /// </summary>
    [JsonPropertyName("contact_inbox")]
    public ChatwootContactInbox ContactInbox { get; set; } = new();
}