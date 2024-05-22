namespace Choco.Backend.Api.Domain.Stocktake.Data;

public class DoStocktakeResponse
{
    public required string ProductTitle { get; set; }
    public int StockBalanceDelta { get; set; }
}