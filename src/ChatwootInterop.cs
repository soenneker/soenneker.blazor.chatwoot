using Microsoft.JSInterop;
using Soenneker.Blazor.Chatwoot.Abstract;
using Soenneker.Blazor.Chatwoot.Configuration;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Utils.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Chatwoot;

///<inheritdoc cref="IChatwootInterop"/>
public sealed class ChatwootInterop : IChatwootInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IResourceLoader _resourceLoader;
    private readonly AsyncSingleton _scriptInitializer;

    private const string _module = "Soenneker.Blazor.Chatwoot/js/chatwootinterop.js";
    private const string _moduleName = "ChatwootInterop";

    public ChatwootInterop(IJSRuntime jsRuntime, IResourceLoader resourceLoader)
    {
        _jsRuntime = jsRuntime;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton(async (token, arr) =>
        {
            if (arr.Length == 0 || arr[0] is not ChatwootConfiguration config)
                throw new ArgumentException("ChatwootConfiguration must be passed to script initializer");

            await _resourceLoader.LoadScriptAndWaitForVariable(config.SdkUrl, "chatwootSDK", cancellationToken: token);
            await _resourceLoader.ImportModuleAndWaitUntilAvailable(_module, _moduleName, 100, token);
            return new object();
        });
    }

    public async ValueTask Init(string elementId, ChatwootConfiguration configuration, DotNetObjectReference<Chatwoot> dotNetReference, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken, configuration);
        string? json = JsonUtil.Serialize(configuration);
        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.init", cancellationToken, elementId, json, dotNetReference);
    }

    public ValueTask Shutdown(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.shutdown", cancellationToken, elementId);
    }

    public ValueTask Toggle(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.toggle", cancellationToken, elementId);
    }

    public ValueTask SetUser(string elementId, string identifier, object attributes, CancellationToken cancellationToken = default)
    {
        string? json = JsonUtil.Serialize(attributes);
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.setUser", cancellationToken, elementId, identifier, json);
    }

    public ValueTask SetUserAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        string? json = JsonUtil.Serialize(attributes);
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.setUserAttributes", cancellationToken, elementId, json);
    }

    public ValueTask SetLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.setLabel", cancellationToken, elementId, label);
    }

    public ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.createObserver", cancellationToken, elementId);
    }

    public ValueTask RemoveLabel(string elementId, string label, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.removeLabel", cancellationToken, elementId, label);
    }

    public ValueTask SetLocale(string elementId, string locale, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.setLocale", cancellationToken, elementId, locale);
    }

    public ValueTask DeleteCustomAttribute(string elementId, string attributeKey, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.deleteCustomAttribute", cancellationToken, elementId, attributeKey);
    }

    public ValueTask Reset(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.reset", cancellationToken, elementId);
    }

    public ValueTask SetCustomAttributes(string elementId, object attributes, CancellationToken cancellationToken = default)
    {
        string? json = JsonUtil.Serialize(attributes);
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.setCustomAttributes", cancellationToken, elementId, json);
    }

    public ValueTask PopoutChatWindow(string elementId, CancellationToken cancellationToken = default)
    {
        return _jsRuntime.InvokeVoidAsync($"{_moduleName}.popoutChatWindow", cancellationToken, elementId);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_module);
        await _scriptInitializer.DisposeAsync();
    }
}
