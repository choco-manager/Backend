using Ardalis.Result;
using Choco.Backend.Api.Common;
using Choco.Backend.Api.Configuration.Swagger;
using Choco.Backend.Api.Domain.Orders.Data;
using Choco.Backend.Api.Domain.Orders.UseCases;
using FastEndpoints;

namespace Choco.Backend.Api.Domain.Orders.Endpoints.UpdatePaymentStatus;

public class UpdatePaymentStatusEndpoint(UpdatePaymentStatusUseCase useCase): Endpoint<UpdatePaymentStatusRequest, Result<IdModel>>
{
    public override void Configure()
    {
        Version(3);
        Patch("/orders/{id}/payment");
        Options(x => x.WithTags(SwaggerTags.Orders));
        DontThrowIfValidationFails();
    }

    public override async Task<Result<IdModel>> ExecuteAsync(UpdatePaymentStatusRequest req, CancellationToken ct)
    {
        return await useCase.Execute(req, ct);
    }
}