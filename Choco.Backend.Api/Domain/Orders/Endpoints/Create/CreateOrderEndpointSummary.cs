using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.Create;

public class CreateOrderEndpointSummary: Summary<CreateOrderEndpoint>
{
    public CreateOrderEndpointSummary()
    {
        Summary = "Creates new order";
    }
}