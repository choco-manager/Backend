using System.Net.Mime;
using Api.Common;
using Api.Configuration.Swagger;
using Api.Domain.Auth.Data;
using Ardalis.Result;
using FastEndpoints;

namespace Api.Domain.Auth.Endpoints.Login;

public class LoginEndpoint(IUseCase<LoginRequest, LoginResponse> loginUseCase)
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