using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.RequestPasswordRestoration;

public class RequestPasswordRestorationEndpoint(RequestPasswordRestorationUseCase requestPasswordRestorationUseCase)
    : Endpoint<UsernameRequest, Result<RestorationTokenResponse>>
{
    public override void Configure()
    {
        Post("auth/request-restoration");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Auth));
        DontThrowIfValidationFails();
        AllowAnonymous();
    }

    public override async Task<Result<RestorationTokenResponse>> ExecuteAsync(UsernameRequest req, CancellationToken ct)
    {
        return await requestPasswordRestorationUseCase.Execute(req, ct);
    }
}