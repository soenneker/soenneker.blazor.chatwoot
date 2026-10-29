using System.Text.Json;
using Microsoft.JSInterop;
using Soenneker.Blazor.Extensions.IJSObjectReferences;
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
            await _scriptInitializer.Init(configuration, linked);
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("init", linked, elementId, JsonSerializer.SerializeToElement(configuration, LibraryJsonContext.Default.ChatwootConfiguration), dotNetReference);
        }
    }

    public async ValueTask Shutdown(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("shutdown", linked, elementId);
        }
    }

    public async ValueTask Toggle(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("toggle", linked, elementId);
        }
    }

    public async ValueTask Open(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("open", linked, elementId);
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
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("setUser", linked, elementId, identifier, attributes);
        }
    }

    public async ValueTask SetUserAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("setUserAttributes", linked, elementId, attributes);
        }
    }

    public async ValueTask SetLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("setLabel", linked, elementId, label);
        }
    }

    public async ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("createObserver", linked, elementId);
        }
    }

    public async ValueTask RemoveLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("removeLabel", linked, elementId, label);
        }
    }

    public async ValueTask SetLocale(string elementId, string locale, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("setLocale", linked, elementId, locale);
        }
    }

    public async ValueTask DeleteCustomAttribute(string elementId, string attributeKey, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("deleteCustomAttribute", linked, elementId, attributeKey);
        }
    }

    public async ValueTask Reset(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("reset", linked, elementId);
        }
    }

    public async ValueTask SetCustomAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("setCustomAttributes", linked, elementId, attributes);
        }
    }

    public async ValueTask PopoutChatWindow(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidWithContextAsync("popoutChatWindow", linked, elementId);
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
