using System.Net.Mime;
using Choco.Backend.Api.Common;
using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Login;

public class LoginEndpoint(LoginUseCase loginUseCase)
    : Endpoint<LoginRequest, Result<LoginResponse>>
{
    public override void Configure()
    {
        Post("auth/login");
        Version(3);
        Options(x =>
        {
            x.WithTags(SwaggerTags.Auth);
            x.Accepts<LoginRequest>(MediaTypeNames.Application.Json);
        });
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task<Result<LoginResponse>> ExecuteAsync(LoginRequest req, CancellationToken ct)
    {
        return await loginUseCase.Execute(req, ct);
    }
}