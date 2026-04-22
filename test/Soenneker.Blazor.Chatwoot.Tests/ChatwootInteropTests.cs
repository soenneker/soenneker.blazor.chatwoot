using Soenneker.Blazor.Chatwoot.Abstract;
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
    public void Default()
    {

    }
}
