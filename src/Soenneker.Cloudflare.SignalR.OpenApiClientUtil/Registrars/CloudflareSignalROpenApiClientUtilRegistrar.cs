using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Cloudflare.SignalR.HttpClients.Registrars;
using Soenneker.Cloudflare.SignalR.OpenApiClientUtil.Abstract;

namespace Soenneker.Cloudflare.SignalR.OpenApiClientUtil.Registrars;

/// <summary>
/// Registers the OpenAPI client utility for dependency injection.
/// </summary>
public static class CloudflareSignalROpenApiClientUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="CloudflareSignalROpenApiClientUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareSignalROpenApiClientUtilAsSingleton(this IServiceCollection services)
    {
        services.AddCloudflareSignalROpenApiHttpClientAsSingleton()
                .TryAddSingleton<ICloudflareSignalROpenApiClientUtil, CloudflareSignalROpenApiClientUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="CloudflareSignalROpenApiClientUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareSignalROpenApiClientUtilAsScoped(this IServiceCollection services)
    {
        services.AddCloudflareSignalROpenApiHttpClientAsSingleton()
                .TryAddScoped<ICloudflareSignalROpenApiClientUtil, CloudflareSignalROpenApiClientUtil>();

        return services;
    }
}
