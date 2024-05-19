using FastEndpoints;

namespace Api.Domain.Auth.Endpoints.RestorePassword;

public class RestorePasswordEndpointSummary: Summary<RestorePasswordEndpoint>
{
    public RestorePasswordEndpointSummary()
    {
        Summary = "Restores user's password";
    }
}