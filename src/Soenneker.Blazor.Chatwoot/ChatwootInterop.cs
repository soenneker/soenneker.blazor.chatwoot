using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Chatwoot.Abstract;
using Soenneker.Blazor.Chatwoot.Configuration;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Chatwoot;

///<inheritdoc cref="IChatwootInterop"/>
public sealed class ChatwootInterop : IChatwootInterop
{
    private readonly IResourceLoader _resourceLoader;
    private readonly IModuleImportUtil _moduleImportUtil;
    private readonly AsyncInitializer<ChatwootConfiguration> _scriptInitializer;

    private const string _wrapperModulePath = "_content/Soenneker.Blazor.Chatwoot/js/chatwootinterop.js";

    private readonly CancellationScope _cancellationScope = new();

    public ChatwootInterop(IResourceLoader resourceLoader, IModuleImportUtil moduleImportUtil)
    {
        _resourceLoader = resourceLoader;
        _moduleImportUtil = moduleImportUtil;

        _scriptInitializer = new AsyncInitializer<ChatwootConfiguration>(Initialize);
    }

    private async ValueTask Initialize(ChatwootConfiguration config, CancellationToken token)
    {
        await _resourceLoader.LoadScriptAndWaitForVariable(config.SdkUrl, "chatwootSDK", cancellationToken: token);
        _ = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, token);
    }

    public async ValueTask Init(string elementId, ChatwootConfiguration configuration, DotNetObjectReference<Chatwoot> dotNetReference,
        CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await _scriptInitializer.Init(configuration, linked);
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("init", linked, elementId, configuration, dotNetReference);
        }
    }

    public async ValueTask Shutdown(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("shutdown", linked, elementId);
        }
    }

    public async ValueTask Toggle(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("toggle", linked, elementId);
        }
    }

    public async ValueTask SetUser(string elementId, string identifier, object attributes, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("setUser", linked, elementId, identifier, attributes);
        }
    }

    public async ValueTask SetUserAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("setUserAttributes", linked, elementId, attributes);
        }
    }

    public async ValueTask SetLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("setLabel", linked, elementId, label);
        }
    }

    public async ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("createObserver", linked, elementId);
        }
    }

    public async ValueTask RemoveLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("removeLabel", linked, elementId, label);
        }
    }

    public async ValueTask SetLocale(string elementId, string locale, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("setLocale", linked, elementId, locale);
        }
    }

    public async ValueTask DeleteCustomAttribute(string elementId, string attributeKey, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("deleteCustomAttribute", linked, elementId, attributeKey);
        }
    }

    public async ValueTask Reset(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("reset", linked, elementId);
        }
    }

    public async ValueTask SetCustomAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("setCustomAttributes", linked, elementId, attributes);
        }
    }

    public async ValueTask PopoutChatWindow(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_wrapperModulePath, linked);
            await module.InvokeVoidAsync("popoutChatWindow", linked, elementId);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_wrapperModulePath);
        await _scriptInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}