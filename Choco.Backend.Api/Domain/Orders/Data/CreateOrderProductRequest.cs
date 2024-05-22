namespace Choco.Backend.Api.Domain.Orders.Data;

public class CreateOrderProductRequest
{
    public Guid Product { get; set; }
    public decimal Amount { get; set; }
}