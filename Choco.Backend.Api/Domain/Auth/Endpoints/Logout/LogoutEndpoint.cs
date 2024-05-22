using Choco.Backend.Api.Common;
using Ardalis.Result;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Auth.Data;
using Choco.Backend.Api.Domain.Auth.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Logout;

public class LogoutEndpoint(LogoutUseCase logoutUseCase)
    : Endpoint<LogoutRequest, Result<EmptyResponse>>
{
    public override void Configure()
    {
        Post("auth/logout");
        Version(3);
        Description(x => x.Accepts<LogoutRequest>());
        Options(x => x.WithTags(SwaggerTags.Auth));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<EmptyResponse>> ExecuteAsync(LogoutRequest req, CancellationToken ct)
    {
        return await logoutUseCase.Execute(
            new AuthorizedLogoutRequest
            {
                User = User,
                AccessToken = req.BearerToken.Split(" ")[1].Trim()
            },
            ct
        );
    }
}