using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.GetAll;

public class GetAllProductsEndpointSummary: Summary<GetAllProductsEndpoint>
{
    public GetAllProductsEndpointSummary()
    {
        Summary = "Gets all products in paged format";
    }
}