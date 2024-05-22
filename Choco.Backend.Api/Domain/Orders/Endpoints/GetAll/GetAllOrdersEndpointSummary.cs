using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.GetAll;

public class GetAllOrdersEndpointSummary: Summary<GetAllOrdersEndpoint>
{
    public GetAllOrdersEndpointSummary()
    {
        Summary = "Gets all orders in paged format";
    }
}