using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Data.Models;

namespace Choco.Backend.Api.Domain.Orders.Data;

public class CreateOrderRequest
{
    public DateTime ToBeDeliveredAt { get; set; }
    public required Address ShippingAddress { get; set; }
    public required ICollection<CreateOrderProductRequest> Products { get; set; }
}