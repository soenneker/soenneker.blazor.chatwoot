using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Chatwoot.Dtos;

/// <summary>
/// Represents the sender of a message in Chatwoot.
/// </summary>
public class ChatwootSender
{
    /// <summary>
    /// Additional attributes about the sender, such as city and IP.
    /// </summary>
    [JsonPropertyName("additional_attributes")]
    public Dictionary<string, object?> AdditionalAttributes { get; set; } = new();

    /// <summary>
    /// Custom attributes assigned to the sender.
    /// </summary>
    [JsonPropertyName("custom_attributes")]
    public Dictionary<string, object?> CustomAttributes { get; set; } = new();

    /// <summary>
    /// The sender’s email address (nullable).
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// The sender’s unique ID.
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }

    /// <summary>
    /// Optional external identifier for the sender.
    /// </summary>
    [JsonPropertyName("identifier")]
    public string? Identifier { get; set; }

    /// <summary>
    /// The name of the sender.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The phone number of the sender (nullable).
    /// </summary>
    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// A thumbnail image or avatar for the sender.
    /// </summary>
    [JsonPropertyName("thumbnail")]
    public string Thumbnail { get; set; } = null!;

    /// <summary>
    /// Indicates whether the sender is blocked.
    /// </summary>
    [JsonPropertyName("blocked")]
    public bool Blocked { get; set; }

    /// <summary>
    /// The sender type (e.g., "contact").
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
}