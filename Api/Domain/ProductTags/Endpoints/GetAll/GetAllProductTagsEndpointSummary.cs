using FastEndpoints;

namespace Api.Domain.ProductTags.Endpoints.GetAll;

public class GetAllProductTagsEndpointSummary: Summary<GetAllProductTagsEndpoint>
{
    public GetAllProductTagsEndpointSummary()
    {
        Summary = "Gets all product tags";
    }
}