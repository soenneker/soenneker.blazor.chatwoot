using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Chatwoot.Configuration;

/// <summary>
/// Represents the general configuration for the Chatwoot widget.
/// </summary>
public sealed class ChatwootConfiguration
{
    /// <summary>
    /// The website token provided by Chatwoot for widget integration.
    /// </summary>
    [JsonPropertyName("websiteToken")]
    public string WebsiteToken { get; set; } = null!;

    /// <summary>
    /// The base URL of the Chatwoot instance.
    /// Default is "https://app.chatwoot.com".
    /// </summary>
    [JsonPropertyName("baseUrl")]
    public string BaseUrl { get; set; } = "https://app.chatwoot.com";

    /// <summary>
    /// The locale (language) for the widget. 
    /// Default is "en".
    /// </summary>
    [JsonPropertyName("locale")]
    public string Locale { get; set; } = "en";

    /// <summary>
    /// Gets the full SDK URL derived from the base URL.
    /// </summary>
    [JsonPropertyName("sdkUrl")]
    public string SdkUrl => $"{BaseUrl.TrimEnd('/')}/packs/js/sdk.js";

    /// <summary>
    /// Determines whether the message bubble is hidden.
    /// Default is false (the bubble is shown).
    /// </summary>
    [JsonPropertyName("hideMessageBubble")]
    public bool HideMessageBubble { get; set; } = false;

    /// <summary>
    /// Controls whether the unread messages dialog is shown.
    /// Default is false (dialog is disabled).
    /// </summary>
    [JsonPropertyName("showUnreadMessagesDialog")]
    public bool ShowUnreadMessagesDialog { get; set; } = false;

    /// <summary>
    /// Sets the position of the widget on the screen.
    /// Valid values: "left", "right".
    /// Default is "right".
    /// </summary>
    [JsonPropertyName("position")]
    public string Position { get; set; } = "right";

    /// <summary>
    /// Determines whether the widget should use the browser's language settings.
    /// Default is false (uses the configured <see cref="Locale"/>).
    /// </summary>
    [JsonPropertyName("useBrowserLanguage")]
    public bool UseBrowserLanguage { get; set; } = false;

    /// <summary>
    /// The type of widget display.
    /// Valid values: "standard", "expanded_bubble".
    /// Default is "standard".
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "standard";

    /// <summary>
    /// Controls dark mode behavior of the widget.
    /// Valid values: "light", "auto".
    /// Default is "auto".
    /// </summary>
    [JsonPropertyName("darkMode")]
    public string DarkMode { get; set; } = "auto";

    /// <summary>
    /// The base domain for tracking users across subdomains.
    /// Optional. Configure only if cross-subdomain tracking is required.
    /// </summary>
    [JsonPropertyName("baseDomain")]
    public string? BaseDomain { get; set; }
}
