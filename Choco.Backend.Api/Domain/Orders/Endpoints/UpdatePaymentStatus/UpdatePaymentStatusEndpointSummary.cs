using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.UpdatePaymentStatus;

public class UpdatePaymentStatusEndpointSummary : Summary<UpdatePaymentStatusEndpoint>
{
    public UpdatePaymentStatusEndpointSummary()
    {
        Summary = "Updates payment status of one exact order";
    }
}