using Choco.Backend.Api.Data.Models;
using Riok.Mapperly.Abstractions;

namespace Choco.Backend.Api.Domain.Orders.Data;

[Mapper]
public partial class OrderMapper
{
    public static partial OrderDto OrderToDto(Order order);
}