using Choco.Backend.Api.Domain.Orders.Endpoints.UpdateOrderStatus;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.UpdatePaymentStatus;

public class UpdatePaymentStatusEndpointSummary : Summary<UpdateOrderStatusEndpoint>
{
    public UpdatePaymentStatusEndpointSummary()
    {
        Summary = "Updates payment status of one exact order";
    }
}