[![](https://img.shields.io/nuget/v/soenneker.cloudflare.signalr.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.signalr.openapiclientutil/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.signalr.openapiclientutil/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.signalr.openapiclientutil/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.cloudflare.signalr.openapiclientutil.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.signalr.openapiclientutil/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Cloudflare.SignalR.OpenApiClientUtil
### A thread-safe utility for obtaining SignalR's OpenApiClient singleton.

## Installation

```
dotnet add package Soenneker.Cloudflare.SignalR.OpenApiClientUtil
```

## Usage

Configure `Relay:Endpoint` with the relay origin and `Relay:AccessKey` with its current signing key. Register `AddCloudflareSignalROpenApiClientUtilAsSingleton()` and inject `ICloudflareSignalROpenApiClientUtil`.

```csharp
var client = await relay.Get(cancellationToken);
var message = new ManagementInvocation
{
    Target = "Changed",
    Arguments = new UntypedArray([new UntypedInteger(42)])
};
await client.Api.Hubs["apphub"].Groups["subscribers"].Send.PostAsync(message, cancellationToken: cancellationToken);

var result = await relay.InvokeAsync("apphub", connectionId,
    new ManagementInvocation { Target = "ReadVersion" }, operationId, cancellationToken);
```

Reuse the same operation ID when retrying the same invocation. A successful invocation returns its JSON result. A void completion returns null. A client error completion throws.
