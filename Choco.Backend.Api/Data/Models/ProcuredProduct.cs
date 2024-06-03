namespace Choco.Backend.Api.Data.Models;

public class ProcuredProduct
{
    public Product Product { get; set; }
    public Guid ProductId { get; set; }
    public Procurement Procurement { get; set; }
    public Guid ProcurementId { get; set; }
    public decimal Amount { get; set; }
}