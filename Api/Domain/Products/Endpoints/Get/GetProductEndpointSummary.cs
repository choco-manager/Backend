using FastEndpoints;

namespace Api.Domain.Products.Endpoints.Get;

public class GetProductEndpointSummary: Summary<GetProductEndpoint>
{
    public GetProductEndpointSummary()
    {
        Summary = "Gets one product by its id";
    }
}