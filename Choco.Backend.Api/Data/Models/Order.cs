using Choco.Backend.Api.Data.Common;
using Choco.Backend.Api.Data.Enums;

namespace Choco.Backend.Api.Data.Models;

public class Order : BaseModel
{
    public required ICollection<OrderedProduct> Products { get; set; }
    public DateTime OrderedAt { get; set; }
    public DateTime ToBeDeliveredAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public required Customer Customer { get; set; }
    public bool IsDeleted { get; set; }
}