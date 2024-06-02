using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.UpdateOrderStatus;

public class UpdateOrderStatusEndpointSummary : Summary<UpdateOrderStatusEndpoint>
{
    public UpdateOrderStatusEndpointSummary()
    {
        Summary = "Updates status of one exact order";
    }
}