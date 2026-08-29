using System;
using System.Threading.Tasks;
using AwesomeAssertions;
using Microsoft.JSInterop;
using Soenneker.Blazor.Chatwoot.Abstract;
using Soenneker.Blazor.Chatwoot.Configuration;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Chatwoot.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class ChatwootInteropTests : HostedUnitTest
{
    private readonly IChatwootInterop _blazorlibrary;

    public ChatwootInteropTests(Host host) : base(host)
    {
        _blazorlibrary = Resolve<IChatwootInterop>(true);
    }

    [Test]
    public async Task Init_rejects_an_insecure_remote_base_url()
    {
        using DotNetObjectReference<Chatwoot> reference = DotNetObjectReference.Create(new Chatwoot());
        var configuration = new ChatwootConfiguration
        {
            WebsiteToken = "website-token",
            BaseUrl = "http://chat.example.com"
        };

        Func<Task> act = async () => await _blazorlibrary.Init("chat", configuration, reference);

        await act.Should().ThrowAsync<ArgumentException>();
    }
}
