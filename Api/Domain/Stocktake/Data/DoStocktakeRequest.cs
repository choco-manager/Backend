namespace Api.Domain.Stocktake.Data;

public class DoStocktakeRequest
{
    public Guid ProductId { get; set; }
    public int StockBalance { get; set; }
}