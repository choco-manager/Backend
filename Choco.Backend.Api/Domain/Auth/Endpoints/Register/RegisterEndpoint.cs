using Choco.Backend.Api.Common;
using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Register;

public class RegisterEndpoint(RegisterUseCase registerUseCase)
    : Endpoint<RegisterRequest, Result<RegisterResponse>>
{
    public override void Configure()
    {
        Post("auth/register");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Auth));
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task<Result<RegisterResponse>> ExecuteAsync(RegisterRequest req, CancellationToken ct)
    {
        return await registerUseCase.Execute(req, ct);
    }
}