using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Soenneker.TestHosts.Unit;
using Soenneker.Utils.Test;
using Soenneker.Cloudflare.SignalR.OpenApiClientUtil.Registrars;

namespace Soenneker.Cloudflare.SignalR.OpenApiClientUtil.Tests;

public sealed class Host : UnitTestHost
{
    public override Task InitializeAsync()
    {
        SetupIoC(Services);

        return base.InitializeAsync();
    }

    private static void SetupIoC(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: false);
        });

        IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new System.Collections.Generic.Dictionary<string, string?>
        {
            ["Relay:Endpoint"] = "http://localhost:8080", ["Relay:AccessKey"] = new string('x', 32)
        }).Build();
        services.AddSingleton(config);

        services.AddCloudflareSignalROpenApiClientUtilAsScoped();
    }
}
