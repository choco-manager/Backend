namespace Choco.Backend.Api.Data.Models;

public class OrderedProduct
{
    public required Product Product { get; set; }
    public Guid ProductId { get; set; }
    public required Order Order { get; set; }
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}