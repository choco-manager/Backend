using Choco.Backend.Api.Data.Enums;

namespace Choco.Backend.Api.Domain.Orders.Data;

public class UpdatePaymentStatusRequest
{
    public Guid Id { get; set; }
    public PaymentStatus Status { get; set; }
}