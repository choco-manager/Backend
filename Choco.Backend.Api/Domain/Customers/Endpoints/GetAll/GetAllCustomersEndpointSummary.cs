using FastEndpoints;

namespace Choco.Backend.Api.Domain.Customers.Endpoints.GetAll;

public class GetAllCustomersEndpointSummary: Summary<GetAllCustomersEndpoint>
{
    public GetAllCustomersEndpointSummary()
    {
        Summary = "Gets all customers in a way suitable for form in the application";
    }
}