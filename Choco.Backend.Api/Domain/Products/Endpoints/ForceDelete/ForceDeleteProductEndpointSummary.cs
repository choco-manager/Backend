using Choco.Backend.Api.Domain.Products.Endpoints.SoftDelete;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.ForceDelete;

public class ForceDeleteProductEndpointSummary: Summary<ForceDeleteProductEndpoint>
{
    public ForceDeleteProductEndpointSummary()
    {
        Summary = "Force deletes product from database";
    }
}