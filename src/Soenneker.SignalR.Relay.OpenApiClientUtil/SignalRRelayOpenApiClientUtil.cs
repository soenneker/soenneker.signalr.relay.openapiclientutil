using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Soenneker.SignalR.Relay.OpenApiClient.Models;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.Extensions.ValueTask;
using Soenneker.SignalR.Relay.HttpClients.Abstract;
using Soenneker.SignalR.Relay.OpenApiClientUtil.Abstract;
using Soenneker.SignalR.Relay.OpenApiClient;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.SignalR.Relay.OpenApiClientUtil;

/// <summary>Provides a shared Kiota client for relay management.</summary>
public sealed class SignalRRelayOpenApiClientUtil : ISignalRRelayOpenApiClientUtil
{
    private readonly AsyncSingleton<SignalRRelayOpenApiClient> _client;

    public SignalRRelayOpenApiClientUtil(ISignalRRelayOpenApiHttpClient httpClientUtil)
    {
        _client = new AsyncSingleton<SignalRRelayOpenApiClient>(async token =>
        {
            HttpClient httpClient = await httpClientUtil.Get(token).NoSync();

            var requestAdapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: httpClient)
            {
                BaseUrl = httpClient.BaseAddress!.GetLeftPart(UriPartial.Authority)
            };

            return new SignalRRelayOpenApiClient(requestAdapter);
        });
    }

    public ValueTask<SignalRRelayOpenApiClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public async ValueTask<JsonElement?> InvokeAsync(string hub, string connectionId, ManagementInvocation invocation,
        Guid? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var client = await Get(cancellationToken).ConfigureAwait(false);
        using var stream = await client.Api.Hubs[hub].Connections[connectionId].Invoke.PostAsync(invocation, request =>
        {
            if (idempotencyKey.HasValue) request.Headers.Add("Idempotency-Key", idempotencyKey.Value.ToString("D"));
        }, cancellationToken).ConfigureAwait(false);
        if (stream is null) throw new InvalidDataException("The relay returned no completion.");
        using var bytes = new MemoryStream();
        await stream.CopyToAsync(bytes, cancellationToken).ConfigureAwait(false);
        var length = checked((int)bytes.Length);
        if (length == 0 || bytes.GetBuffer()[length - 1] != 0x1e) throw new InvalidDataException("Missing completion terminator.");
        using var doc = JsonDocument.Parse(bytes.GetBuffer().AsMemory(0, length - 1));
        var frame = doc.RootElement;
        if (frame.GetProperty("type").GetInt32() != 3) throw new InvalidDataException("Expected a client completion.");
        if (frame.TryGetProperty("error", out var error)) throw new InvalidOperationException(error.GetString());
        return frame.TryGetProperty("result", out var result) ? result.Clone() : null;
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
