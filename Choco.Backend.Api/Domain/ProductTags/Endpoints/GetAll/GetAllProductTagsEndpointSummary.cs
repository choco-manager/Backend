using FastEndpoints;

namespace Choco.Backend.Api.Domain.ProductTags.Endpoints.GetAll;

public class GetAllProductTagsEndpointSummary: Summary<GetAllProductTagsEndpoint>
{
    public GetAllProductTagsEndpointSummary()
    {
        Summary = "Gets all product tags";
    }
}