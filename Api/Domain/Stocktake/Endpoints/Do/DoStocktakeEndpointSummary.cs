using FastEndpoints;

namespace Api.Domain.Stocktake.Endpoints.Do;

public class DoStocktakeEndpointSummary: Summary<DoStocktakeEndpoint>
{
    public DoStocktakeEndpointSummary()
    {
        Summary = "Does stocktaking of one product";
    }
}