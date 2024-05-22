using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Internal.Endpoints.Ping;

public class PingEndpoint : Endpoint<EmptyRequest, Result<string>>
{
    public override void Configure()
    {
        Get("/ping");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Internal));
        AllowAnonymous();
    }

    public override async Task<Result<string>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return Result.Success("pong");
    }
}