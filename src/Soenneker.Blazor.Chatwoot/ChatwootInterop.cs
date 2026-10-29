using System.Text.Json;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Chatwoot.Abstract;
using Soenneker.Blazor.Chatwoot.Configuration;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Chatwoot;

/// <inheritdoc cref="IChatwootInterop"/>
public sealed class ChatwootInterop : IChatwootInterop
{
    private readonly IResourceLoader _resourceLoader;
    private readonly IModuleImportUtil _moduleImportUtil;
    private readonly AsyncInitializer<ChatwootConfiguration> _scriptInitializer;

    private const string _wrapperModulePath = "./_content/Soenneker.Blazor.Chatwoot/js/chatwootinterop.js";

    private readonly CancellationScope _cancellationScope = new();

    public ChatwootInterop(IResourceLoader resourceLoader, IModuleImportUtil moduleImportUtil)
    {
        _resourceLoader = resourceLoader;
        _moduleImportUtil = moduleImportUtil;

        _scriptInitializer = new AsyncInitializer<ChatwootConfiguration>(Initialize);
    }

    private async ValueTask Initialize(ChatwootConfiguration config, CancellationToken token)
    {
        ValidateConfiguration(config);
        await _resourceLoader.LoadScriptAndWaitForVariable(config.SdkUrl, "chatwootSDK", cancellationToken: token);
        _ = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, token);
    }

    public async ValueTask Init(string elementId, ChatwootConfiguration configuration, DotNetObjectReference<Chatwoot> dotNetReference,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(elementId);
        ArgumentNullException.ThrowIfNull(dotNetReference);
        ValidateConfiguration(configuration);

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                await _scriptInitializer.Init(configuration, linked);
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("init", linked, elementId, JsonSerializer.SerializeToElement(configuration, LibraryJsonContext.Default.ChatwootConfiguration), dotNetReference);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.Init' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask Shutdown(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("shutdown", linked, elementId);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.Shutdown' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask Toggle(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("toggle", linked, elementId);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.Toggle' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask Open(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("open", linked, elementId);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.Open' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask Close(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("close", linked, elementId);
            }
            catch (OperationCanceledException) when (linked.IsCancellationRequested)
            {
            }
        }
    }

    public async ValueTask SetUser(string elementId, string identifier, object attributes, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("setUser", linked, elementId, identifier, attributes);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.{identifier}' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask SetUserAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("setUserAttributes", linked, elementId, attributes);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.SetUserAttributes' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask SetLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("setLabel", linked, elementId, label);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.SetLabel' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("createObserver", linked, elementId);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.CreateObserver' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask RemoveLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("removeLabel", linked, elementId, label);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.RemoveLabel' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask SetLocale(string elementId, string locale, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("setLocale", linked, elementId, locale);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.SetLocale' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask DeleteCustomAttribute(string elementId, string attributeKey, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("deleteCustomAttribute", linked, elementId, attributeKey);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.DeleteCustomAttribute' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask Reset(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("reset", linked, elementId);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.Reset' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask SetCustomAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("setCustomAttributes", linked, elementId, attributes);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.SetCustomAttributes' failed: {exception.Message}", exception);
            }
        }
    }

    public async ValueTask PopoutChatWindow(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            try
            {
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
                await module.InvokeVoidAsync("popoutChatWindow", linked, elementId);
            }
            catch (JSException exception)
            {
                throw new JSException($"JavaScript operation 'ChatwootInterop.PopoutChatWindow' failed: {exception.Message}", exception);
            }
        }
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        _cancellationScope.Cancel();
        await _moduleImportUtil.DisposeContentModule(_wrapperModulePath);
        await _scriptInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }

    private static void ValidateConfiguration(ChatwootConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.WebsiteToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.BaseUrl);

        if (!Uri.TryCreate(configuration.BaseUrl, UriKind.Absolute, out Uri? baseUri))
            throw new ArgumentException("Chatwoot BaseUrl must be an absolute URI.", nameof(configuration));

        bool isHttps = string.Equals(baseUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
        bool isLoopbackHttp = baseUri.IsLoopback && string.Equals(baseUri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase);

        if (!isHttps && !isLoopbackHttp)
            throw new ArgumentException("Chatwoot BaseUrl must use HTTPS unless it is a loopback HTTP URI.", nameof(configuration));

        if (!string.IsNullOrEmpty(baseUri.UserInfo))
            throw new ArgumentException("Chatwoot BaseUrl cannot contain credentials.", nameof(configuration));
    }
}
