using Soenneker.Blazor.Chatwoot.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Blazor.Chatwoot.Tests;

[Collection("Collection")]
public class ChatwootInteropTests : FixturedUnitTest
{
    private readonly IChatwootInterop _blazorlibrary;

    public ChatwootInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _blazorlibrary = Resolve<IChatwootInterop>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
