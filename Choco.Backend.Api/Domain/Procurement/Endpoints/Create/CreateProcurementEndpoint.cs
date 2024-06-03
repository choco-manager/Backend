using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Procurement.Data;
using Choco.Backend.Api.Domain.Procurement.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Procurement.Endpoints.Create;

public class CreateProcurementEndpoint(CreateProcurementUseCase useCase)
    : Endpoint<CreateProcurementRequest, Result<IdModel>>
{
    public override void Configure()
    {
        Post("procurements");
        Version(3);
        Options(x => x.WithTags(SwaggerTags.Procurements));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<IdModel>> ExecuteAsync(CreateProcurementRequest req, CancellationToken ct)
    {
        return await useCase.Execute(req, ct);
    }
}