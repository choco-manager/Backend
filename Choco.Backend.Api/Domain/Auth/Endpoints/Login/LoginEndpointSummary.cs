using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Login;

public class LoginEndpointSummary: Summary<LoginEndpoint>
{
    public LoginEndpointSummary()
    {
        Summary = "Authorizes user, issuing new access token and refresh token";
    }
}