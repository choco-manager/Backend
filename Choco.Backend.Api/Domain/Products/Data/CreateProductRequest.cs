namespace Choco.Backend.Api.Domain.Products.Data;

public class CreateProductRequest
{
    public required string Title { get; set; }
    public required decimal RetailPrice { get; set; }
    public required decimal CostPrice { get; set; }
    public required List<Guid> Tags { get; set; }
    public required int StockBalance { get; set; }
    public required bool IsBulk { get; set; }
}