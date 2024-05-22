using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Data.Models;

namespace Choco.Backend.Api.Domain.Orders.Data;

public class OrderDto
{
    public Guid Id { get; set; }
    public required ICollection<OrderedProductDto> Products { get; set; }
    public DateTime OrderedAt { get; set; }
    public DateTime ToBeDeliveredAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public required Address ShippingAddress { get; set; }
    public decimal TotalAmount { get; set; }
}