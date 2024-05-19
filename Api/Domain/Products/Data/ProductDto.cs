namespace Api.Domain.Products.Data;

public class ProductDto
{
    public required string Title { get; set; }
    public decimal RetailPrice { get; set; }
    public decimal CostPrice { get; set; }
    public required List<string> Tags { get; set; }
    public required int StockBalance { get; set; }
    public required bool IsBulk { get; set; }
}