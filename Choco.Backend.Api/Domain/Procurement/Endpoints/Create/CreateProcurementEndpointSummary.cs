using FastEndpoints;

namespace Choco.Backend.Api.Domain.Procurement.Endpoints.Create;

public class CreateProcurementEndpointSummary : Summary<CreateProcurementEndpoint>
{
    public CreateProcurementEndpointSummary()
    {
        Summary = "Creates new procurement";
    }
}