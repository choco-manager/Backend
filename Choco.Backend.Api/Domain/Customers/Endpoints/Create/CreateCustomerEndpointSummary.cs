using FastEndpoints;

namespace Choco.Backend.Api.Domain.Customers.Endpoints.Create;

public class CreateCustomerEndpointSummary: Summary<CreateCustomerEndpoint>
{
    public CreateCustomerEndpointSummary()
    {
        Summary = "Creates new customer";
    }
}