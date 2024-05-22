using Choco.Backend.Api.Common;
using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Refresh;

public class RefreshEndpoint(RefreshUseCase refreshUseCase): Endpoint<RefreshRequest, Result<LoginResponse>>
{
    public override void Configure()
    {
        Post("auth/refresh");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Auth));
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task<Result<LoginResponse>> ExecuteAsync(RefreshRequest req, CancellationToken ct)
    {
        return await refreshUseCase.Execute(req, ct);
    }
}