namespace Choco.Backend.Api.Domain.Products.Data;

public class UpdateProductRequest
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public decimal? RetailPrice { get; set; }
    public decimal? CostPrice { get; set; }
    public List<Guid>? Tags { get; set; }
    public bool? IsBulk { get; set; }
}