using Microsoft.JSInterop;
using Soenneker.Blazor.Chatwoot.Abstract;
using Soenneker.Blazor.Chatwoot.Configuration;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Asyncs.Initializers;

namespace Soenneker.Blazor.Chatwoot;

///<inheritdoc cref="IChatwootInterop"/>
public sealed class ChatwootInterop : IChatwootInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IResourceLoader _resourceLoader;
    private readonly AsyncInitializer<ChatwootConfiguration> _scriptInitializer;

    private const string _module = "Soenneker.Blazor.Chatwoot/js/chatwootinterop.js";
    private const string _moduleName = "ChatwootInterop";

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

    public async ValueTask Init(string elementId, ChatwootConfiguration configuration, DotNetObjectReference<Chatwoot> dotNetReference,
        CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(configuration, cancellationToken);
        await _jsRuntime.InvokeVoidAsync("ChatwootInterop.init", cancellationToken, elementId, configuration, dotNetReference);
    }

    public ValueTask Shutdown(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.shutdown", cancellationToken, elementId);
    }

    public ValueTask Toggle(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.toggle", cancellationToken, elementId);
    }

    public ValueTask SetUser(string elementId, string identifier, object attributes, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setUser", cancellationToken, elementId, identifier, attributes);
    }

    public ValueTask SetUserAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setUserAttributes", cancellationToken, elementId, attributes);
    }

    public ValueTask SetLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setLabel", cancellationToken, elementId, label);
    }

    public ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.createObserver", cancellationToken, elementId);
    }

    public ValueTask RemoveLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.removeLabel", cancellationToken, elementId, label);
    }

    public ValueTask SetLocale(string elementId, string locale, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setLocale", cancellationToken, elementId, locale);
    }

    public ValueTask DeleteCustomAttribute(string elementId, string attributeKey, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.deleteCustomAttribute", cancellationToken, elementId, attributeKey);
    }

    public ValueTask Reset(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.reset", cancellationToken, elementId);
    }

    public ValueTask SetCustomAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.setCustomAttributes", cancellationToken, elementId, attributes);
    }

    public ValueTask PopoutChatWindow(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync("ChatwootInterop.popoutChatWindow", cancellationToken, elementId);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_module);
        await _scriptInitializer.DisposeAsync();
    }
}