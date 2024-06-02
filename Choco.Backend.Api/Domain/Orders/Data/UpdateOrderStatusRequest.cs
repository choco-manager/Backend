using Choco.Backend.Api.Data.Enums;

namespace Choco.Backend.Api.Domain.Orders.Data;

public class UpdateOrderStatusRequest
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
}