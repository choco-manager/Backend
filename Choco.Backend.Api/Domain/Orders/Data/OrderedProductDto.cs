namespace Choco.Backend.Api.Domain.Orders.Data;

public class OrderedProductDto
{
    public required string ProductName { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
}