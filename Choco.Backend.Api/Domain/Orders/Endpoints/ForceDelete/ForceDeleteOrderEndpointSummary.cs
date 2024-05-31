using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.ForceDelete;

public class ForceDeleteOrderEndpointSummary: Summary<ForceDeleteOrderEndpoint>
{
    public ForceDeleteOrderEndpointSummary()
    {
        Summary = "Force deletes order (without chance to restore)";
    }
}