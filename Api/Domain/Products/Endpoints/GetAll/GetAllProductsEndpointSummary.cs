using FastEndpoints;

namespace Api.Domain.Products.Endpoints.GetAll;

public class GetAllProductsEndpointSummary: Summary<GetAllProductsEndpoint>
{
    public GetAllProductsEndpointSummary()
    {
        Summary = "Gets all products in paged format";
    }
}