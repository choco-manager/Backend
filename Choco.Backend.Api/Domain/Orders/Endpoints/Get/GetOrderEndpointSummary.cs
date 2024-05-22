using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.Get;

public class GetOrderEndpointSummary: Summary<GetOrderEndpoint>
{
    public GetOrderEndpointSummary()
    {
        Summary = "Gets order by id";
    }
}