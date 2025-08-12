using Microsoft.AspNetCore.Components;
using Soenneker.Blazor.Chatwoot.Dtos;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Quark.Components.Cancellable.Abstract;

namespace Soenneker.Blazor.Chatwoot.Abstract;

/// <summary>
/// Represents the Chatwoot live chat Blazor component with full interop functionality.
/// </summary>
public interface IChatwoot : ICancellableComponent
{
    /// <summary>
    /// Invoked when the Chatwoot widget is ready.
    /// </summary>
    EventCallback OnReady { get; set; }

    /// <summary>
    /// Invoked when the Chatwoot widget is opened by the user.
    /// </summary>
    EventCallback OnOpen { get; set; }

    /// <summary>
    /// Invoked when the Chatwoot widget is closed by the user.
    /// </summary>
    EventCallback OnClose { get; set; }

    /// <summary>
    /// Invoked when a new message is received from the Chatwoot widget.
    /// </summary>
    EventCallback<ChatwootMessage> OnMessage { get; set; }

    /// <summary>
    /// Invoked when the Chatwoot widget encounters an error.
    /// </summary>
    EventCallback<JsonElement> OnError { get; set; }

    /// <summary>
    /// Shuts down the Chatwoot widget instance.
    /// </summary>
    ValueTask Shutdown(CancellationToken cancellationToken = default);

    /// <summary>
    /// Toggles the Chatwoot widget visibility.
    /// </summary>
    ValueTask Toggle(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the user with a unique identifier and attributes.
    /// </summary>
    /// <param name="identifier">A unique string to identify the user.</param>
    /// <param name="attributes">A JSON-serializable object of attributes.</param>
    ValueTask SetUser(string identifier, object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates user attributes without changing the identifier.
    /// </summary>
    /// <param name="attributes">A JSON-serializable object of attributes.</param>
    ValueTask SetUserAttributes(object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a label to the current conversation.
    /// </summary>
    /// <param name="label">The label to apply.</param>
    ValueTask SetLabel(string label, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a MutationObserver to monitor DOM changes for the Chatwoot widget.
    /// </summary>
    ValueTask CreateObserver(CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a label from the current conversation.
    /// </summary>
    /// <param name="label">The label to remove.</param>
    ValueTask RemoveLabel(string label, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the locale (language) of the Chatwoot widget.
    /// </summary>
    /// <param name="locale">The locale string, e.g., "en", "fr".</param>
    ValueTask SetLocale(string locale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a previously set custom user attribute.
    /// </summary>
    /// <param name="attributeKey">The key of the attribute to delete.</param>
    ValueTask DeleteCustomAttribute(string attributeKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the Chatwoot session, removing any stored user or conversation data.
    /// </summary>
    ValueTask Reset(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets multiple custom user attributes.
    /// </summary>
    /// <param name="attributes">A JSON-serializable object of custom attributes.</param>
    ValueTask SetCustomAttributes(object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens the Chatwoot widget in a separate browser window (popout).
    /// </summary>
    ValueTask PopoutChatWindow(CancellationToken cancellationToken = default);

    /// <summary>
    /// Callback from JavaScript indicating that Chatwoot is fully loaded and ready.
    /// </summary>
    Task OnReadyCallback();

    /// <summary>
    /// Callback from JavaScript triggered when the chat widget is opened.
    /// </summary>
    Task OnOpenCallback();

    /// <summary>
    /// Callback from JavaScript triggered when the chat widget is closed.
    /// </summary>
    Task OnCloseCallback();

    /// <summary>
    /// Callback from JavaScript triggered when a new message is received.
    /// </summary>
    /// <param name="args">The JSON message payload.</param>
    Task OnMessageCallback(JsonElement args);

    /// <summary>
    /// Callback from JavaScript triggered when an error occurs.
    /// </summary>
    /// <param name="args">The JSON error payload.</param>
    Task OnErrorCallback(JsonElement args);
}