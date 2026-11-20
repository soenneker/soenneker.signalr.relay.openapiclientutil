using Soenneker.Cloudflare.SignalR.OpenApiClientUtil.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Cloudflare.SignalR.OpenApiClientUtil.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class CloudflareSignalROpenApiClientUtilTests : HostedUnitTest
{
    private readonly ICloudflareSignalROpenApiClientUtil _openapiclientutil;

    public CloudflareSignalROpenApiClientUtilTests(Host host) : base(host)
    {
        _openapiclientutil = Resolve<ICloudflareSignalROpenApiClientUtil>(true);
    }

    [Test]
    public async System.Threading.Tasks.Task SharesOneClientAndBuildsGroupSendRequests()
    {
        var client = await _openapiclientutil.Get();
        await Assert.That(object.ReferenceEquals(client, await _openapiclientutil.Get())).IsTrue();
        await Assert.That(client.Api.Hubs["sample"].Groups["one"].Send.ToPostRequestInformation(
            new global::Soenneker.Cloudflare.SignalR.OpenApiClient.Models.ManagementInvocation { Target = "Changed" }).UrlTemplate)
            .Contains("/api/hubs/{hub}/groups/{group}/:send");
    }
}
