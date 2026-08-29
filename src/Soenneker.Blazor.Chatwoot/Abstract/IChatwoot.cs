using Microsoft.AspNetCore.Components;
using Soenneker.Blazor.Chatwoot.Dtos;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Lepton.Suite.Abstract;

namespace Soenneker.Blazor.Chatwoot.Abstract;

/// <summary>
/// Represents the Chatwoot live chat Blazor component with full interop functionality.
/// </summary>
public interface IChatwoot : ILeptonCancellableIdentifiableContentElement
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
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the shutdown operation is complete.</returns>
    ValueTask Shutdown(CancellationToken cancellationToken = default);

    /// <summary>
    /// Toggles the Chatwoot widget visibility.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the toggle operation is complete.</returns>
    ValueTask Toggle(CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens the Chatwoot widget.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the open operation is complete.</returns>
    ValueTask Open(CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes the Chatwoot widget without resetting the current session.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the close operation is complete.</returns>
    ValueTask Close(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the user with a unique identifier and attributes.
    /// </summary>
    /// <param name="identifier">A unique string to identify the user.</param>
    /// <param name="attributes">A JSON-serializable object of attributes.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the user has been stored.</returns>
    ValueTask SetUser(string identifier, object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates user attributes without changing the identifier.
    /// </summary>
    /// <param name="attributes">A JSON-serializable object of attributes.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the user attributes has been stored.</returns>
    ValueTask SetUserAttributes(object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a label to the current conversation.
    /// </summary>
    /// <param name="label">Human-readable label to display.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the label has been stored.</returns>
    ValueTask SetLabel(string label, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a MutationObserver to monitor DOM changes for the Chatwoot widget.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the observer creation is complete.</returns>
    ValueTask CreateObserver(CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a label from the current conversation.
    /// </summary>
    /// <param name="label">Human-readable label to display.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the label removal is complete.</returns>
    ValueTask RemoveLabel(string label, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the locale (language) of the Chatwoot widget.
    /// </summary>
    /// <param name="locale">The locale string, e.g., "en", "fr".</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the locale has been stored.</returns>
    ValueTask SetLocale(string locale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a previously set custom user attribute.
    /// </summary>
    /// <param name="attributeKey">Attribute Key for the delete custom attribute operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes after the targeted files have been deleted.</returns>
    ValueTask DeleteCustomAttribute(string attributeKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the Chatwoot session, removing any stored user or conversation data.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the reset operation is complete.</returns>
    ValueTask Reset(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets multiple custom user attributes.
    /// </summary>
    /// <param name="attributes">A JSON-serializable object of custom attributes.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the custom attributes has been stored.</returns>
    ValueTask SetCustomAttributes(object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens the Chatwoot widget in a separate browser window (popout).
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the popout chat window operation is complete.</returns>
    ValueTask PopoutChatWindow(CancellationToken cancellationToken = default);

    /// <summary>
    /// Callback from JavaScript indicating that Chatwoot is fully loaded and ready.
    /// </summary>
    /// <returns>A task that completes when the on ready callback operation is complete.</returns>
    Task OnReadyCallback();

    /// <summary>
    /// Callback from JavaScript triggered when the chat widget is opened.
    /// </summary>
    /// <returns>A task that completes when the on open callback operation is complete.</returns>
    Task OnOpenCallback();

    /// <summary>
    /// Callback from JavaScript triggered when the chat widget is closed.
    /// </summary>
    /// <returns>A task that completes when the on close callback operation is complete.</returns>
    Task OnCloseCallback();

    /// <summary>
    /// Callback from JavaScript triggered when a new message is received.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the application.</param>
    /// <returns>A task that completes when the on message callback operation is complete.</returns>
    Task OnMessageCallback(JsonElement args);

    /// <summary>
    /// Callback from JavaScript triggered when an error occurs.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the application.</param>
    /// <returns>A task that completes when the on error callback operation is complete.</returns>
    Task OnErrorCallback(JsonElement args);
}
