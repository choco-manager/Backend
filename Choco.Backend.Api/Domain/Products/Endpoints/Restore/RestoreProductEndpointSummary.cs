using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.Restore;

public class RestoreProductEndpointSummary: Summary<RestoreProductEndpoint>
{
    public RestoreProductEndpointSummary()
    {
        Summary = "Restores deleted product";
    }
}