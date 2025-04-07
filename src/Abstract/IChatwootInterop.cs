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
    ValueTask Init(string elementId, ChatwootConfiguration configuration, DotNetObjectReference<Chatwoot> dotNetReference, CancellationToken cancellationToken = default);

    /// <summary>
    /// Shuts down the Chatwoot widget.
    /// </summary>
    ValueTask Shutdown(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Toggles the Chatwoot widget's visibility.
    /// </summary>
    ValueTask Toggle(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the user identifier and attributes.
    /// </summary>
    ValueTask SetUser(string elementId, string identifier, object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates user attributes for the current session.
    /// </summary>
    ValueTask SetUserAttributes(string elementId, object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a label on the current conversation.
    /// </summary>
    ValueTask SetLabel(string elementId, string label, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates an observer to track DOM changes.
    /// </summary>
    ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a label from the current conversation.
    /// </summary>
    ValueTask RemoveLabel(string elementId, string label, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the locale (language) of the Chatwoot widget.
    /// </summary>
    ValueTask SetLocale(string elementId, string locale, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a custom user attribute.
    /// </summary>
    ValueTask DeleteCustomAttribute(string elementId, string attributeKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the current Chatwoot session.
    /// </summary>
    ValueTask Reset(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets custom attributes for the current user.
    /// </summary>
    ValueTask SetCustomAttributes(string elementId, object attributes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens the Chatwoot widget in a popout window.
    /// </summary>
    ValueTask PopoutChatWindow(string elementId, CancellationToken cancellationToken = default);
}
