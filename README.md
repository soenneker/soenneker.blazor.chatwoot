[![](https://img.shields.io/nuget/v/soenneker.blazor.chatwoot.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.chatwoot/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.chatwoot/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.chatwoot/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.chatwoot.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.chatwoot/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.chatwoot/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.chatwoot/actions/workflows/codeql.yml)

# Soenneker.Blazor.Chatwoot

A Blazor component and scoped JavaScript interop service for the Chatwoot website widget.

## Installation and registration

```bash
dotnet add package Soenneker.Blazor.Chatwoot
```

```csharp
using Soenneker.Blazor.Chatwoot.Registrars;

builder.Services.AddChatwootInteropAsScoped();
```

## Render the widget

```razor
@using Soenneker.Blazor.Chatwoot.Configuration
@using Soenneker.Blazor.Chatwoot.Dtos

<Chatwoot @ref="_chatwoot"
          Configuration="_configuration"
          OnReady="OnReady"
          OnOpen="OnOpen"
          OnClose="OnClose"
          OnMessage="OnMessage"
          OnError="OnError" />

@code {
    private Chatwoot? _chatwoot;

    private readonly ChatwootConfiguration _configuration = new()
    {
        WebsiteToken = "your-website-token",
        BaseUrl = "https://app.chatwoot.com",
        Locale = "en",
        Position = "right",
        DarkMode = "auto"
    };

    private Task OnReady() => Task.CompletedTask;
    private Task OnOpen() => Task.CompletedTask;
    private Task OnClose() => Task.CompletedTask;
    private Task OnMessage(ChatwootMessage message) => Task.CompletedTask;
    private Task OnError(JsonElement error) => Task.CompletedTask;
}
```

The component loads the SDK and creates the widget after its first render. `BaseUrl` must be absolute HTTPS; loopback HTTP is allowed for local development. Chatwoot's SDK is page-global, so render only one `Chatwoot` component at a time.

## Control the widget

Use the component reference after `OnReady` has fired:

```csharp
await _chatwoot!.Open();
await _chatwoot.Close();
await _chatwoot.Toggle();

await _chatwoot.SetUser("customer-42", new
{
    name = "Ada Lovelace",
    email = "ada@example.com",
    identifier_hash = hashFromYourServer
});

await _chatwoot.SetCustomAttributes(new
{
    plan = "enterprise",
    region = "us-central"
});
```

The component also exposes `SetUserAttributes`, `SetLabel`, `RemoveLabel`, `SetLocale`, `DeleteCustomAttribute`, `Reset`, `Shutdown`, and `PopoutChatWindow`. `Close` hides the widget while retaining its session; `Reset` clears the Chatwoot session; `Shutdown` removes this wrapper's event handlers and resets the widget.

For authenticated visitors, enable Chatwoot identity validation and generate `identifier_hash` on a trusted server. Never place the HMAC secret in the Blazor application. Attribute values and chat content are sent to the configured Chatwoot instance, so apply the same consent and privacy controls used for other third-party customer-data tools.

If the application uses a Content Security Policy, allow the configured Chatwoot origin for its SDK, frames, images, and network connections. The component releases its callbacks and widget state when it is removed.
