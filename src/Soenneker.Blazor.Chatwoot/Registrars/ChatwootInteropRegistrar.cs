using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Chatwoot.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.Chatwoot.Registrars;

/// <summary>
/// A Blazor interop library for Chatwoot, the open-source customer engagement suite.
/// </summary>
public static class ChatwootInteropRegistrar
{
    /// <summary>
    /// Adds <see cref="IChatwootInterop"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddChatwootInteropAsScoped(this IServiceCollection services)
    {
        services.AddResourceLoaderAsScoped().TryAddScoped<IChatwootInterop, ChatwootInterop>();

        return services;
    }
}
