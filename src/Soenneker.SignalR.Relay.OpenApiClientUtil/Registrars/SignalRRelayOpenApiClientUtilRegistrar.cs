using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.SignalR.Relay.HttpClients.Registrars;
using Soenneker.SignalR.Relay.OpenApiClientUtil.Abstract;

namespace Soenneker.SignalR.Relay.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the OpenAPI client utility for dependency injection.
/// </summary>
public static class SignalRRelayOpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="SignalRRelayOpenApiClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddSignalRRelayOpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddSignalRRelayOpenApiHttpClientAsSingleton()
                .TryAddSingleton<ISignalRRelayOpenApiClientUtil, SignalRRelayOpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="SignalRRelayOpenApiClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddSignalRRelayOpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddSignalRRelayOpenApiHttpClientAsSingleton()
                .TryAddScoped<ISignalRRelayOpenApiClientUtil, SignalRRelayOpenApiClientUtil>();

        return services;
    }
}
