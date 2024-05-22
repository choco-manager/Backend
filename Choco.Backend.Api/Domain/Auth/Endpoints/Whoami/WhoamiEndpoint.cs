using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Whoami;

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