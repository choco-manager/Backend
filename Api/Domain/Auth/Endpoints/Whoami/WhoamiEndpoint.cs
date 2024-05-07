using Api.Common;
using Api.Configuration.Swagger;
using Api.Domain.Auth.Data;
using Api.Domain.Auth.UseCases;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Auth.Endpoints.Whoami;

public class WhoamiEndpoint(WhoamiUseCase whoamiUseCase)
    : Endpoint<EmptyRequest, Result<WhoamiResponse>>
{
    public override void Configure()
    {
        Get("auth/whoami");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Auth));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<WhoamiResponse>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return await whoamiUseCase.Execute(new AuthorizedEmptyRequest { User = User }, ct);
    }
}