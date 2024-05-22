using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.SoftDelete;

public class SoftDeleteProductEndpointSummary: Summary<SoftDeleteProductEndpoint>
{
    public SoftDeleteProductEndpointSummary()
    {
        Summary = "Marks product as deleted, without real deleting";
    }
}