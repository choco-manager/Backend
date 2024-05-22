using FastEndpoints;

namespace Choco.Backend.Api.Domain.Auth.Endpoints.Register;

public class RegisterEndpointSummary: Summary<RegisterEndpoint>
{
    public RegisterEndpointSummary()
    {
        Summary = "Registers new user";
    }
}