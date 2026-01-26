using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Chatwoot.Abstract;
using Soenneker.Blazor.Chatwoot.Configuration;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Chatwoot;

///<inheritdoc cref="IChatwootInterop"/>
public sealed class ChatwootInterop : IChatwootInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IResourceLoader _resourceLoader;
    private readonly AsyncInitializer<ChatwootConfiguration> _scriptInitializer;

    private const string _module = "Soenneker.Blazor.Chatwoot/js/chatwootinterop.js";
    private const string _moduleName = "ChatwootInterop";

    private readonly CancellationScope _cancellationScope = new();

    public ChatwootInterop(IJSRuntime jsRuntime, IResourceLoader resourceLoader)
    {
        _jsRuntime = jsRuntime;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncInitializer<ChatwootConfiguration>(Initialize);
    }

    private async ValueTask Initialize(ChatwootConfiguration config, CancellationToken token)
    {
        await _resourceLoader.LoadScriptAndWaitForVariable(config.SdkUrl, "chatwootSDK", cancellationToken: token);
        await _resourceLoader.ImportModuleAndWaitUntilAvailable(_module, _moduleName, 100, token);
    }

    public async ValueTask Init(
        string elementId,
        ChatwootConfiguration configuration,
        DotNetObjectReference<Chatwoot> dotNetReference,
        CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(configuration, linked);
            await _jsRuntime.InvokeVoidAsync("ChatwootInterop.init", linked, elementId, configuration, dotNetReference);
        }
    }

    public ValueTask Shutdown(string elementId, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.shutdown", linked, elementId);
    }

    public ValueTask Toggle(string elementId, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.toggle", linked, elementId);
    }

    public ValueTask SetUser(string elementId, string identifier, object attributes, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setUser", linked, elementId, identifier, attributes);
    }

    public ValueTask SetUserAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setUserAttributes", linked, elementId, attributes);
    }

    public ValueTask SetLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setLabel", linked, elementId, label);
    }

    public ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.createObserver", linked, elementId);
    }

    public ValueTask RemoveLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.removeLabel", linked, elementId, label);
    }

    public ValueTask SetLocale(string elementId, string locale, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setLocale", linked, elementId, locale);
    }

    public ValueTask DeleteCustomAttribute(string elementId, string attributeKey, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.deleteCustomAttribute", linked, elementId, attributeKey);
    }

    public ValueTask Reset(string elementId, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.reset", linked, elementId);
    }

    public ValueTask SetCustomAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setCustomAttributes", linked, elementId, attributes);
    }

    public ValueTask PopoutChatWindow(string elementId, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
            return _jsRuntime.InvokeVoidAsync("ChatwootInterop.popoutChatWindow", linked, elementId);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_module);
        await _scriptInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}
