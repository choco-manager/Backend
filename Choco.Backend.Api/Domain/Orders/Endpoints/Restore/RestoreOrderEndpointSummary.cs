using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.Restore;

public class RestoreOrderEndpointSummary: Summary<RestoreOrderEndpoint>
{
    public RestoreOrderEndpointSummary()
    {
        Summary = "Restores soft deleted order";
    }
}