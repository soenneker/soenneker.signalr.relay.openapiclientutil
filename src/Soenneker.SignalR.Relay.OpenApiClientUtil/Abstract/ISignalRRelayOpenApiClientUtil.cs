using Soenneker.SignalR.Relay.OpenApiClient;
using System;
using System.Text.Json;
using Soenneker.SignalR.Relay.OpenApiClient.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.SignalR.Relay.OpenApiClientUtil.Abstract;

/// <summary>
/// Exposes a cached OpenAPI client instance.
/// </summary>
public interface ISignalRRelayOpenApiClientUtil: IDisposable, IAsyncDisposable
{
    /// <summary>Gets the generated relay management client.</summary>
    /// <param name="cancellationToken">Cancels initialization.</param>
    /// <returns>The shared client.</returns>
    ValueTask<SignalRRelayOpenApiClient> Get(CancellationToken cancellationToken = default);

    /// <summary>Invokes a client method and decodes its framed SignalR completion.</summary>
    /// <param name="hub">Hub name.</param>
    /// <param name="connectionId">Target client connection.</param>
    /// <param name="invocation">Method and ordered JSON arguments.</param>
    /// <param name="idempotencyKey">Optional key for replaying the result of the same request.</param>
    /// <param name="cancellationToken">Cancels the request.</param>
    /// <returns>The JSON result, or null for a void completion.</returns>
    /// <exception cref="InvalidOperationException">The client returned an error completion.</exception>
    ValueTask<JsonElement?> InvokeAsync(string hub, string connectionId, ManagementInvocation invocation,
        Guid? idempotencyKey = null, CancellationToken cancellationToken = default);
}
