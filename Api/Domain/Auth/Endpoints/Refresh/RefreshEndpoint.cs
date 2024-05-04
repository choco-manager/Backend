using Api.Common;
using Api.Configuration.Swagger;
using Api.Domain.Auth.Data;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Auth.Endpoints.Refresh;

public class RefreshEndpoint(IUseCase<RefreshRequest, LoginResponse> refreshUseCase): Endpoint<RefreshRequest, Result<LoginResponse>>
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