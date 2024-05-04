namespace Api.Domain.Products.Data;

public class ProductDto
{
    public required string Title { get; set; }
    public required decimal RetailPrice { get; set; }
    public required decimal CostPrice { get; set; }
    public required List<string> Tags { get; set; }
    public required int StockBalance { get; set; }
    public required bool IsBulk { get; set; }
}