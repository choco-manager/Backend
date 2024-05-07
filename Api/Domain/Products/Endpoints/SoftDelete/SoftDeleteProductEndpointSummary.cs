using FastEndpoints;

namespace Api.Domain.Products.Endpoints.SoftDelete;

public class SoftDeleteProductEndpointSummary: Summary<SoftDeleteProductEndpoint>
{
    public SoftDeleteProductEndpointSummary()
    {
        Summary = "Помечает товар удалённым, не удаляя его насовсем";
    }
}