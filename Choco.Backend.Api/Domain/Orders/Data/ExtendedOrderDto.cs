using Choco.Backend.Api.Data.Enums;
using Choco.Backend.Api.Data.Models;

namespace Choco.Backend.Api.Domain.Orders.Data;

public class ExtendedOrderDto: OrderDto
{
    public required ICollection<OrderedProductDto> Products { get; set; }
}