using Api.Common;
using Api.Configuration.Swagger;
using Api.Domain.Auth.Data;
using Ardalis.Result;
using FastEndpoints;
using Microsoft.Net.Http.Headers;

namespace Api.Domain.Auth.Endpoints.Logout;

public class LogoutEndpoint(IUseCase<AuthorizedLogoutRequest, EmptyResponse> logoutUseCase)
    : Endpoint<EmptyRequest, Result<EmptyResponse>>
{
    public override void Configure()
    {
        Post("auth/logout");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Auth));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<EmptyResponse>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return await logoutUseCase.Execute(
            new AuthorizedLogoutRequest
            {
                User = User,
                AccessToken = HttpContext.Request.Headers[HeaderNames.Authorization].First()!.Split(" ")[1].Trim()
            },
            ct
        );
    }
}