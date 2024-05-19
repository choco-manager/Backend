using Api.Data.Common;
using Api.Data.Enums;

namespace Api.Data.Models;

public class Order : BaseModel
{
    public required ICollection<OrderedProduct> Products { get; set; }
    public DateTime OrderedAt { get; set; }
    public DateTime ToBeDeliveredAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public required Address ShippingAddress { get; set; }
    public decimal TotalAmount { get; set; }
}