using FastEndpoints;

namespace Choco.Backend.Api.Domain.Products.Endpoints.Update;

public class UpdateProductEndpointSummary: Summary<UpdateProductEndpoint>
{
    public UpdateProductEndpointSummary()
    {
        Summary = "Updates product by id";
    }
}