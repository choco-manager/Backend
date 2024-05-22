using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Whoami;

public class WhoamiEndpointSummary: Summary<WhoamiEndpoint>
{
    public WhoamiEndpointSummary()
    {
        Summary = "Retrieves information about current user";
    }
}