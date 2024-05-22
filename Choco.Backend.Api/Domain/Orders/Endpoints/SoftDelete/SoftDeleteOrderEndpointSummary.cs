using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.SoftDelete;

public class SoftDeleteOrderEndpointSummary : Summary<SoftDeleteOrderEndpoint>
{
    public SoftDeleteOrderEndpointSummary()
    {
        Summary = "Marks order as deleted, without real deleting";
    }
}