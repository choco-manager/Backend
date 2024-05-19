using Api.Configuration.Swagger;
using Api.Domain.Auth.Data;
using Api.Domain.Auth.UseCases;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Auth.Endpoints.RestorePassword;

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