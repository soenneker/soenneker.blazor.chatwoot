[![](https://img.shields.io/nuget/v/soenneker.blazor.chatwoot.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.chatwoot/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.chatwoot/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.chatwoot/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.chatwoot.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.chatwoot/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Chatwoot

### A Blazor interop library for [Chatwoot](https://www.chatwoot.com/), the open-source customer engagement suite.

---

## ✨ Features

- 📦 Lightweight Blazor component wrapper for the Chatwoot JS SDK
- 🔄 Full .NET interop with JavaScript events
- 📡 Supports event callbacks like `OnOpen`, `OnMessage`, and `OnError`
- ⚙️ Clean integration using dependency injection
- 🧪 Supports unit testing with `IChatwoot` abstraction

---

## 📦 Installation

```bash
dotnet add package Soenneker.Blazor.Chatwoot
```

Register the interop in DI:

```csharp
public static async Task Main(string[] args)
{
    builder.Services.AddChatwootInteropAsScoped();
}
```

---

## 🚀 Usage

### 🧩 Add to a Razor component

```razor
<Chatwoot Configuration="_config"
          OnOpen="HandleOpen"
          OnClose="HandleClose"
          OnReady="HandleReady"
          OnMessage="HandleMessage"
          OnError="HandleError" />
```

### 🧠 Component code-behind

```csharp
@code {
    private readonly ChatwootConfiguration _config = new()
    {
        WebsiteToken = "replace-with-your-token",
        BaseUrl = "https://app.chatwoot.com"
    };

    private Task HandleReady() => ConsoleLog("Chatwoot is ready!");
    private Task HandleOpen() => ConsoleLog("Chat opened");
    private Task HandleClose() => ConsoleLog("Chat closed");

    private Task HandleMessage(ChatwootMessage message)
    {
        Console.WriteLine($"Message from Chatwoot: {message.Content}");
        return Task.CompletedTask;
    }

    private Task HandleError(JsonElement error)
    {
        Console.WriteLine($"Chatwoot error: {error}");
        return Task.CompletedTask;
    }

    private Task ConsoleLog(string msg)
    {
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }
}
```

---

## ⚙️ Configuration

You must supply a `ChatwootConfiguration` object to the component:

```csharp
var config = new ChatwootConfiguration
{
    WebsiteToken = "your-token", // Required
    BaseUrl = "https://app.chatwoot.com", // Optional, defaults to this
    Locale = "en", // Optional, default is "en"
    HideMessageBubble = false,
    ShowUnreadMessagesDialog = false,
    Position = "right", // "left" or "right"
    UseBrowserLanguage = false,
    Type = "standard", // or "expanded_bubble"
    DarkMode = "auto", // "light" or "auto"
    BaseDomain = null // Optional, for cross-subdomain tracking
};
```


## 📚 API

This library provides a full interface via `IChatwoot`, including:

- `SetUser(...)`
- `SetLabel(...)`
- `Shutdown()`
- `Toggle()`
- `SetLocale(...)`
- `SetCustomAttributes(...)`
- ... and more!

---

## 💬 Chatwoot Events

The following Chatwoot events are exposed as Blazor `EventCallback`s:

| Chatwoot Event       | .NET Callback           |
|----------------------|--------------------------|
| `chatwoot:ready`     | `OnReady`                |
| `chatwoot:open`      | `OnOpen`                 |
| `chatwoot:close`     | `OnClose`                |
| `chatwoot:on-message`| `OnMessage(ChatwootMessage)` |
| `chatwoot:error`     | `OnError(JsonElement)`   |
