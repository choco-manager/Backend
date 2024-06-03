using Choco.Backend.Api.Domain.Orders.Data;

namespace Choco.Backend.Api.Domain.Procurement.Data;

public class CreateProcurementRequest
{
    public required ICollection<CreateOrderProductRequest> Products { get; set; }
}