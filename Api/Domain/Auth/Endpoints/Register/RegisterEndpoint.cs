using Api.Common;
using Api.Configuration.Swagger;
using Api.Domain.Auth.Data;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Auth.Endpoints.Register;

public class RegisterEndpoint(IUseCase<RegisterRequest, RegisterResponse> registerUseCase)
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