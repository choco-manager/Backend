using FastEndpoints;

namespace Api.Domain.Products.Endpoints.Update;

public class UpdateProductEndpointSummary: Summary<UpdateProductEndpoint>
{
    public UpdateProductEndpointSummary()
    {
        Summary = "Updates product by id";
    }
}