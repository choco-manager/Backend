using FastEndpoints;

namespace Choco.Backend.Api.Domain.Stocktake.Endpoints.Do;

public class DoStocktakeEndpointSummary: Summary<DoStocktakeEndpoint>
{
    public DoStocktakeEndpointSummary()
    {
        Summary = "Does stocktaking of one product";
    }
}