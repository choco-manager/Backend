namespace Choco.Backend.Api.Domain.Stocktake.Data;

public class DoStocktakeRequest
{
    public Guid Id { get; set; }
    public int StockBalance { get; set; }
}