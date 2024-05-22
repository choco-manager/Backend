using FastEndpoints;

namespace Choco.Backend.Api.Domain.Internal.Endpoints.Ping;

public class PingEndpointSummary : Summary<PingEndpoint>
{
    public PingEndpointSummary()
    {
        Summary = """Unconditionally returns "pong" string""";
    }
}