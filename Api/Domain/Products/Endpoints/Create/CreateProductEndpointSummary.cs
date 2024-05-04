using FastEndpoints;

namespace Api.Domain.Products.Endpoints.Create;

public class CreateProductEndpointSummary : Summary<CreateProductEndpoint>
{
    public CreateProductEndpointSummary()
    {
        Summary = "Creates new product";
    }
}