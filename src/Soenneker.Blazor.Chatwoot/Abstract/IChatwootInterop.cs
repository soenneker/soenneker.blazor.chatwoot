using Microsoft.JSInterop;
using Soenneker.Blazor.Chatwoot.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Chatwoot.Abstract;

/// <summary>
/// Interface for Chatwoot Blazor interop wrapper
/// </summary>
public interface IChatwootInterop : IAsyncDisposable
{
    /// <summary>
    /// Initializes the Chatwoot widget on a specific DOM element.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="configuration">configuration that supplies runtime settings.</param>
    /// <param name="dotNetReference">JavaScript-invokable reference to the .NET component instance.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the chatwoot is ready for use.</returns>
    /// <exception cref="ArgumentException">Thrown when the element ID, website token, or base URL is invalid, or when the base URL is insecure.</exception>
    ValueTask Init(string elementId, ChatwootConfiguration configuration, DotNetObjectReference<Chatwoot> dotNetReference, CancellationToken cancellationToken = default);

    /// <summary>
    /// Shuts down the Chatwoot widget.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the shutdown operation is complete.</returns>
    ValueTask Shutdown(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Toggles the Chatwoot widget's visibility.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the toggle operation is complete.</returns>
    ValueTask Toggle(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens the Chatwoot widget, waiting briefly for the SDK to finish attaching if necessary.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the open operation is complete.</returns>
    ValueTask Open(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes the Chatwoot widget without resetting the current session.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the close operation is complete.</returns>
    ValueTask Close(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the user identifier and attributes.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="identifier">Identifier of the target value.</param>
    /// <param name="attributes">Attributes for the set user operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the user has been stored.</returns>
    ValueTask SetUser(string elementId, string identifier, object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets user Attributes.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="attributes">Attributes for the set user attributes operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the user attributes has been stored.</returns>
    ValueTask SetUserAttributes(string elementId, object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a label on the current conversation.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="label">Human-readable label to display.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the label has been stored.</returns>
    ValueTask SetLabel(string elementId, string label, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an observer to track DOM changes.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the observer creation is complete.</returns>
    ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a label from the current conversation.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="label">Human-readable label to display.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the label removal is complete.</returns>
    ValueTask RemoveLabel(string elementId, string label, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the locale (language) of the Chatwoot widget.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="locale">Locale for the set locale operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the locale has been stored.</returns>
    ValueTask SetLocale(string elementId, string locale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a custom user attribute.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="attributeKey">Attribute Key for the delete custom attribute operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes after the targeted files have been deleted.</returns>
    ValueTask DeleteCustomAttribute(string elementId, string attributeKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the current Chatwoot session.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the reset operation is complete.</returns>
    ValueTask Reset(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets custom Attributes.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="attributes">Attributes for the set custom attributes operation.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the custom attributes has been stored.</returns>
    ValueTask SetCustomAttributes(string elementId, object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens the Chatwoot widget in a popout window.
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the popout chat window operation is complete.</returns>
    ValueTask PopoutChatWindow(string elementId, CancellationToken cancellationToken = default);
}
