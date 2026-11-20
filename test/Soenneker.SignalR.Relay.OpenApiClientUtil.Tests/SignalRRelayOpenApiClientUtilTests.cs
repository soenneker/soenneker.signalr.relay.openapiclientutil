using Soenneker.SignalR.Relay.OpenApiClientUtil.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.SignalR.Relay.OpenApiClientUtil.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class SignalRRelayOpenApiClientUtilTests : HostedUnitTest
{
    private readonly ISignalRRelayOpenApiClientUtil _openapiclientutil;

    public SignalRRelayOpenApiClientUtilTests(Host host) : base(host)
    {
        _openapiclientutil = Resolve<ISignalRRelayOpenApiClientUtil>(true);
    }

    [Test]
    public async System.Threading.Tasks.Task SharesOneClientAndBuildsGroupSendRequests()
    {
        var client = await _openapiclientutil.Get();
        await Assert.That(object.ReferenceEquals(client, await _openapiclientutil.Get())).IsTrue();
        await Assert.That(client.Api.Hubs["sample"].Groups["one"].Send.ToPostRequestInformation(
            new global::Soenneker.SignalR.Relay.OpenApiClient.Models.ManagementInvocation { Target = "Changed" }).UrlTemplate)
            .Contains("/api/hubs/{hub}/groups/{group}/:send");
    }
}
