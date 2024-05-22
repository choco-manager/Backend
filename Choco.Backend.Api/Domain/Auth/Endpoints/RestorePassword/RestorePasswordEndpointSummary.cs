using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.RestorePassword;

public class RestorePasswordEndpointSummary: Summary<RestorePasswordEndpoint>
{
    public RestorePasswordEndpointSummary()
    {
        Summary = "Restores user's password";
    }
}