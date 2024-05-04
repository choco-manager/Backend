using FastEndpoints;

namespace Api.Domain.Auth.Endpoints.Register;

public class RegisterEndpointSummary: Summary<RegisterEndpoint>
{
    public RegisterEndpointSummary()
    {
        Summary = "Registers new user";
    }
}