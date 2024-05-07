using FastEndpoints;

namespace Api.Domain.Products.Endpoints.Restore;

public class RestoreProductEndpointSummary: Summary<RestoreProductEndpoint>
{
    public RestoreProductEndpointSummary()
    {
        Summary = "Восстанавливает удалённый товар";
    }
}