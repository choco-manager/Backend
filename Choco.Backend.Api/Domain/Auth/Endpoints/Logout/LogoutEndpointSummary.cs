using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Logout;

public class LogoutEndpointSummary: Summary<LogoutEndpoint>
{
    public LogoutEndpointSummary()
    {
        Summary = "Logs out current user and revokes his access and refresh tokens";
    }
}