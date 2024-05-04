using Api.Configuration.Swagger;
using FastEndpoints;

namespace Api.Domain.Internal.Endpoints.Ping;

public class PingEndpoint : Endpoint<EmptyRequest, string>
{
    public override void Configure()
    {
        Get("/ping");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Internal));
        AllowAnonymous();
    }

    public override async Task<string> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return "pong";
    }
}