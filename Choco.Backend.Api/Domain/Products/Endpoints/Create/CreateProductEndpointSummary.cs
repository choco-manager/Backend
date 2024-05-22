using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.Create;

public class CreateProductEndpointSummary : Summary<CreateProductEndpoint>
{
    public CreateProductEndpointSummary()
    {
        Summary = "Creates new product";
    }
}