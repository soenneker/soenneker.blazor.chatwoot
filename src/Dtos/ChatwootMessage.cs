using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Chatwoot.Dtos;

/// <summary>
/// Represents a message sent or received in a Chatwoot conversation.
/// </summary>
public sealed class ChatwootMessage
{
    /// <summary>
    /// The unique identifier of the message.
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// The raw text content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;

    /// <summary>
    /// The Chatwoot account ID the message belongs to.
    /// </summary>
    [JsonPropertyName("account_id")]
    public int AccountId { get; set; }

    /// <summary>
    /// The inbox ID where the message was received.
    /// </summary>
    [JsonPropertyName("inbox_id")]
    public int InboxId { get; set; }

    /// <summary>
    /// The conversation ID associated with the message.
    /// </summary>
    [JsonPropertyName("conversation_id")]
    public int ConversationId { get; set; }

    /// <summary>
    /// The type of message: 0 = incoming, 1 = outgoing.
    /// </summary>
    [JsonPropertyName("message_type")]
    public int MessageType { get; set; }

    /// <summary>
    /// The UNIX timestamp of when the message was created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public long CreatedAtUnix { get; set; }

    /// <summary>
    /// The UTC DateTime representation of the message creation timestamp.
    /// </summary>
    [JsonIgnore]
    public DateTime CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnix).UtcDateTime;

    /// <summary>
    /// The timestamp when the message was last updated.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Indicates whether the message is private (internal note).
    /// </summary>
    [JsonPropertyName("private")]
    public bool Private { get; set; }

    /// <summary>
    /// The status of the message (e.g., "sent").
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;

    /// <summary>
    /// Optional source ID if provided.
    /// </summary>
    [JsonPropertyName("source_id")]
    public string? SourceId { get; set; }

    /// <summary>
    /// The type of content in the message, such as "text".
    /// </summary>
    [JsonPropertyName("content_type")]
    public string ContentType { get; set; } = null!;

    /// <summary>
    /// Additional content metadata.
    /// </summary>
    [JsonPropertyName("content_attributes")]
    public Dictionary<string, object?> ContentAttributes { get; set; } = new();

    /// <summary>
    /// The type of sender, typically "Contact".
    /// </summary>
    [JsonPropertyName("sender_type")]
    public string SenderType { get; set; } = null!;

    /// <summary>
    /// The sender ID.
    /// </summary>
    [JsonPropertyName("sender_id")]
    public long SenderId { get; set; }

    /// <summary>
    /// Optional external source IDs.
    /// </summary>
    [JsonPropertyName("external_source_ids")]
    public Dictionary<string, object?> ExternalSourceIds { get; set; } = new();

    /// <summary>
    /// Optional additional attributes provided with the message.
    /// </summary>
    [JsonPropertyName("additional_attributes")]
    public Dictionary<string, object?> AdditionalAttributes { get; set; } = new();

    /// <summary>
    /// Processed version of the message content.
    /// </summary>
    [JsonPropertyName("processed_message_content")]
    public string ProcessedMessageContent { get; set; } = null!;

    /// <summary>
    /// Optional sentiment analysis data.
    /// </summary>
    [JsonPropertyName("sentiment")]
    public Dictionary<string, object?> Sentiment { get; set; } = new();

    /// <summary>
    /// The conversation context the message belongs to.
    /// </summary>
    [JsonPropertyName("conversation")]
    public ChatwootConversation Conversation { get; set; } = new();

    /// <summary>
    /// The sender of the message.
    /// </summary>
    [JsonPropertyName("sender")]
    public ChatwootSender Sender { get; set; } = new();
}
