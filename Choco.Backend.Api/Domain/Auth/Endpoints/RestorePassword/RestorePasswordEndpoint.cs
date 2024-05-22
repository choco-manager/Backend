using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.RestorePassword;

public class RestorePasswordEndpoint(RestorePasswordUseCase restorePasswordUseCase)
    : Endpoint<RestorePasswordRequest, Result<LoginResponse>>
{
    public override void Configure()
    {
        Post("auth/restore");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Auth));
        DontThrowIfValidationFails();
        AllowAnonymous();
    }

    public override async Task<Result<LoginResponse>> ExecuteAsync(RestorePasswordRequest req, CancellationToken ct)
    {
        return await restorePasswordUseCase.Execute(req, ct);
    }
}